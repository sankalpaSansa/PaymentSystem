using Abp.UI;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using PaymentSystem.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentSystem.Repo
{
    public class TopupRepo: ITopupRepo
    {
        private readonly EntityDBContex _context;

        public TopupRepo(EntityDBContex context)
        {
            _context = context;
        }

        public decimal TopupBalance(TopupDto input)
        {
            var user = _context.Users.FirstOrDefault(f => f.UserId == input.UserId);
            if (user == null)
            {
                throw new UserFriendlyException("User not found with UserId " + input.UserId);
            }
            var wallet = _context.Wallets.FirstOrDefault(f => f.User.UserId == input.UserId);
            if (wallet != null)
            {
                wallet.Amount = wallet.Amount + input.TopupAmount;
                wallet.UpdatedOn = DateTime.Now;
                var topUpWallet = _context.Update(wallet);
                _context.SaveChanges();
                //transaction(input, user);
                return topUpWallet.Entity.Amount;
            }

            Wallet walletNew = new Wallet
            {
                Amount = input.TopupAmount,
                User = user,
                UpdatedOn = DateTime.Now
            };
            var newBalance = _context.Add(walletNew);
            _context.SaveChanges();
            //transaction(input, user);
            return newBalance.Entity.Amount;
        }

    }
}
