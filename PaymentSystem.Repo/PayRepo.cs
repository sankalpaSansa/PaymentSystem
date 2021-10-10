using Abp.UI;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using PaymentSystem.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentSystem.Repo
{
    public class PayRepo : IPayRepo
    {
        private readonly EntityDBContex _context;

        public PayRepo(EntityDBContex context)
        {
            _context = context;
        }
        public BalanceDto GetBalance(Guid id)
        {
           var wallet = _context.Wallets.FirstOrDefault(f => f.User.UserId == id);
            if (wallet == null)
            {
                return new BalanceDto
                {
                    Amount = 0,
                    DebtFromList = null,
                    DebtToList = null,
                };
            }

            var debtFrom = debtFromList(id);
            var debtTo = debtToList(id);

            return new BalanceDto
            {
                Amount = wallet.Amount,
                DebtFromList = debtFrom,
                DebtToList = debtTo,
            };
        }

        public object GetAllWallets()
        {
           return _context.Wallets.ToList();
        }

        public BalanceDto FundTransfer(TransferDto input)       
        {
            var payeeUser = _context.Users.FirstOrDefault(f => f.Username == input.PayeeUserName);
            if (payeeUser == null)
            {
                throw new UserFriendlyException("User not found with UserId " + input.PayeeUserName);
            }
            var userWallet = _context.Wallets.Include(a => a.User).FirstOrDefault(f => f.User.UserId == input.Id);
            if(userWallet == null)
            {
                throw new UserFriendlyException("No wallet created for user " + input.Id);
            }
            var debtUserFrom = _context.DebtFromStatuses.Include(a => a.User).Include(a => a.DebtFrom)
                .FirstOrDefault(f => f.DebtFrom.Username == input.PayeeUserName && f.User.UserId == input.Id);
            if (debtUserFrom != null && debtUserFrom.DebtFrom.Username == input.PayeeUserName)
                return handleDebtTransfer(input, userWallet);

            decimal balnce = userWallet.Amount - input.TransferAmount;

            if(balnce >= 0)
            {
                userWallet.Amount = balnce;
                userWallet.UpdatedOn = DateTime.Now;
                _context.Update(userWallet);
                _context.SaveChanges();

                List<DebtFromDto> debtFrom = new List<DebtFromDto>();
                List<DebtToDto> debtTo = new List<DebtToDto>();

                var debtToCheck = _context.DebtToStatuses.Include(a => a.User).Include(a => a.DebtTo)
                .FirstOrDefault(f => f.DebtTo.Username == input.PayeeUserName && f.User.UserId == input.Id);
                if(debtToCheck != null && debtToCheck.Amount != 0)
                {
                    var debtsToLis = TransferUpdateDebtTo(input, balnce);
                    var debstFromLis = TransferUpdateDebtFrom(input, balnce);
                }

                updateBalanceOfPayee(input, payeeUser);
                debtFrom = debtFromList(input.Id);
                debtTo = debtToList(input.Id);
                return new BalanceDto
                {
                    Amount = balnce,
                    DebtFromList = debtFrom,
                    DebtToList = debtTo
                };
            }

            userWallet.Amount = 0; // Math.Abs(balnce);
            _context.Update(userWallet);
            _context.SaveChanges();

            var debtToLis = TransferUpdateDebtTo(input, balnce);
            var debtFromLis = TransferUpdateDebtFrom(input, balnce);

            updateBalanceOfPayee(input, payeeUser);

            return new BalanceDto
            {
                Amount = 0,
                DebtFromList = debtFromLis,
                DebtToList = debtToLis
            };
        }

        private BalanceDto handleDebtTransfer(TransferDto input, Wallet wallet)
        {
            var userPay = _context.Users.FirstOrDefault(f => f.UserId == input.Id);
            var userRec = _context.Users.FirstOrDefault(f => f.Username == input.PayeeUserName);
            TransferDto modifyIn = new TransferDto()
            {
                Id = userRec.UserId,
                PayeeUserName = userPay.Username,
                TransferAmount = input.TransferAmount
            };

            var debtToLis = TransferUpdateDebtTo(modifyIn, input.TransferAmount);
            var debtFromLis = TransferUpdateDebtFrom(modifyIn, input.TransferAmount);
            var debtFrom = debtFromList(input.Id);
            var debtTo = debtToList(input.Id);

            return new BalanceDto
            {
                Amount = wallet.Amount,
                DebtFromList = debtFrom,
                DebtToList = debtTo
            };
        }
        private List<DebtFromDto> debtFromList(Guid id)
        {
            List<DebtFromDto> debtFromsList = new List<DebtFromDto>();
            
           var debtFroms = _context.DebtFromStatuses.Include(a => a.User).Include(a => a.DebtFrom)
                .Where(f => f.User.UserId == id).ToList();
            if (debtFroms.Count > 0)
            {
                foreach(var debt in debtFroms)
                {
                    var userEn = _context.Users.FirstOrDefault(f => f.UserId == debt.DebtFrom.UserId);
                    var debts = new DebtFromDto()
                    {
                        DebtFromUserName = debt.DebtFrom.Username,
                        DebtFromAmount = debt.Amount
                    };
                    debtFromsList.Add(debts);
                }

                return debtFromsList;
            }

            return null;
        }

        private List<DebtToDto> debtToList(Guid id)
        {
            List<DebtToDto> debtToList = new List<DebtToDto>();

            var debtTo = _context.DebtToStatuses.Include(a => a.User).Include(a => a.DebtTo)
                .Where(f => f.User.UserId == id).ToList();
            if (debtTo.Count > 0)
            {
                foreach (var debt in debtTo)
                {
                    var debts = new DebtToDto()
                    {
                        DebtToUserName = debt.DebtTo.Username,
                        DebtToAmount = debt.Amount
                    };
                    debtToList.Add(debts);
                }

                return debtToList;
            }

            return null;
        }

        private List<DebtToDto> TransferUpdateDebtTo(TransferDto input, decimal val)
        {
            List<DebtToDto> debtList = new List<DebtToDto>();
            var debtTo = _context.DebtToStatuses.Include(a => a.User).Include(a => a.DebtTo)
                .FirstOrDefault(f => f.DebtTo.Username == input.PayeeUserName && f.User.UserId == input.Id);
            var logInUser = _context.Users.FirstOrDefault(f => f.UserId == input.Id);
            var payeeUser = _context.Users.FirstOrDefault(f => f.Username == input.PayeeUserName);

            if (debtTo != null)
            {
                debtTo.Amount = debtTo.Amount - input.TransferAmount;
                _context.Update(debtTo);
                _context.SaveChanges();

               return debtToList(input.Id);
            }

            DebtToStatus debtToStatus = new DebtToStatus
            {
                User = logInUser,
                DebtTo = payeeUser,
                Amount = Math.Abs(val),
                UpdatedOn = DateTime.Now
            };

            _context.Add(debtToStatus);
            _context.SaveChanges();
            return debtToList(input.Id);
        }

        private List<DebtFromDto> TransferUpdateDebtFrom(TransferDto input, decimal val)
        {
            List<DebtFromDto> debtList = new List<DebtFromDto>();
            var debtFrom = _context.DebtFromStatuses.Include(a => a.User).Include(a => a.DebtFrom)
                .FirstOrDefault(f => f.DebtFrom.UserId == input.Id && f.User.Username == input.PayeeUserName);
            var logInUser = _context.Users.FirstOrDefault(f => f.UserId == input.Id);
            var payeeUser = _context.Users.FirstOrDefault(f => f.Username == input.PayeeUserName);

            if (debtFrom != null)
            {
                var banceDebtFrom = debtFrom.Amount - input.TransferAmount;
                if(banceDebtFrom >= 0)
                {
                    debtFrom.Amount = banceDebtFrom;
                }
                else
                {
                    var userWallet = _context.Wallets.FirstOrDefault(f => f.User.UserId == input.Id);

                    userWallet.Amount = Math.Abs(banceDebtFrom);
                    userWallet.UpdatedOn = DateTime.Now;
                    _context.Update(debtFrom);
                    _context.SaveChanges();

                    debtFrom.Amount = 0;

                }
                debtFrom.UpdatedOn = DateTime.Now;
                _context.Update(debtFrom);
                _context.SaveChanges();

                return debtFromList(input.Id);
            }

            DebtFromStatus debtToStatus = new DebtFromStatus
            {
                User = payeeUser,
                DebtFrom = logInUser,
                Amount = Math.Abs(val),
                UpdatedOn = DateTime.Now
            };

            _context.Add(debtToStatus);
            _context.SaveChanges();
            return debtFromList(input.Id);
        }
        private void updateBalanceOfPayee(TransferDto input, User payeeUser)
        {           
            var userWallet = _context.Wallets.FirstOrDefault(f => f.User.Username == input.PayeeUserName);
            var debtToTable = _context.DebtToStatuses.Include(a => a.User).Include(a => a.DebtTo)
                .FirstOrDefault(f => f.DebtTo.Username == input.PayeeUserName && f.User.UserId == input.Id);

            if (userWallet.Amount >= 0 
                && (debtToTable == null || debtToTable.Amount == 0))
            {
                userWallet.Amount = userWallet.Amount + input.TransferAmount;
                userWallet.UpdatedOn = DateTime.Now;

                _context.Update(userWallet);
                _context.SaveChanges();
                return;
            }

                decimal addAmount = input.TransferAmount - debtToTable.Amount;
            if(addAmount >= 0)
            {
                userWallet.Amount = userWallet.Amount + addAmount;
                userWallet.UpdatedOn = DateTime.Now;

                _context.Update(userWallet);
                _context.SaveChanges();
                return;
            }
            else
            {
                userWallet.Amount = userWallet.Amount + input.TransferAmount;
                userWallet.UpdatedOn = DateTime.Now;

                _context.Update(userWallet);
                _context.SaveChanges();
                return;
            }

        }
    }
}
