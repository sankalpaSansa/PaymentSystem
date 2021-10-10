using PaymentSystem.Client.API;
using PaymentSystem.Repo.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Client.Business
{
    public class CommandLine
    {
        private ApiAccess _api=null;
        private UserDto _currentUser = null;

        public void Run()
        {
            _api = new ApiAccess();
            string currentInput = string.Empty;
            while (currentInput.ToLower() != "e")
            {
                Console.Write(">");
                currentInput = Console.ReadLine();
                ProcessInput(currentInput);
            }
        }

        private void ProcessInput(string input)
        {
            string[] keyWordsArr = input.Split(' ');

            if (keyWordsArr[0] == "login")
            {
                LoginUser(keyWordsArr);
            }
            else
            {
                if (!IsUserLoggedIn())
                {
                    return;
                }

                switch (keyWordsArr[0])
                {
                    case "topup":                       
                        TopUp(keyWordsArr);
                        break;
                    case "pay":
                        Transfer(keyWordsArr, false);
                        break;
                    default:
                        Console.WriteLine("Command not found");
                        break;
                }
            }

        }

        private void LoginUser(string[] keywordArray)
        {
            if(keywordArray[1] == "" || keywordArray.Length != 2)
            {
                Console.WriteLine("Error While Login, Please use correct format");
                return;
            }
                
            UserDto user = _api.LoginUserAsync(keywordArray[1]).GetAwaiter().GetResult();
            if(user==null)
            {
                Console.WriteLine("Error While Login, Please try again");
            }
            else
            {
                _currentUser = user;
                BalanceDto balance = _api.GetBalanceAsync(_currentUser.UserId.ToString()).GetAwaiter().GetResult();
                ShowSaliutation();
                ShowBalance(balance);
            }
        }

        private void TopUp(string[] keywordArray)
        {
            if (keywordArray[1] == "" || keywordArray.Length != 2)
            {
                Console.WriteLine("Error While Login, Please use correct format");
                return;
            }
            decimal amount=-1;
            Decimal.TryParse(keywordArray[1], out amount);
            if(amount<=0)
            {
                Console.WriteLine("Amount should be positive number");
                return;
            }
            bool topUpSuccess = _api.TopUpAsync(_currentUser.UserId, amount).GetAwaiter().GetResult();
            if(topUpSuccess)
            {
                BalanceDto balance = _api.GetBalanceAsync(_currentUser.UserId.ToString()).GetAwaiter().GetResult();
                if(!CheckForDebtTansfer(balance, amount))
                    ShowBalance(balance);
            }
            else
            {
                Console.WriteLine("TopUp Failed. Please Retry.");
            }
        }

        private void Transfer(string[] keywordArray, bool topupTrans)
        {
            decimal amount = -1;
            Decimal.TryParse(keywordArray[2], out amount);
            if (amount <= 0)
            {
                Console.WriteLine("Amount should be positive number");
                return;
            }
            BalanceDto balanceEarly = _api.GetBalanceAsync(_currentUser.UserId.ToString()).GetAwaiter().GetResult();
            if (balanceEarly.DebtToList != null && balanceEarly.DebtToList.Count > 0 && !topupTrans)
            {
                foreach (DebtToDto debtTo in balanceEarly.DebtToList)
                {
                    if (debtTo.DebtToAmount > 0)
                    {
                        Console.WriteLine("Owing " + debtTo.DebtToAmount + " to " + debtTo.DebtToUserName + " not allow to pay.");
                        return;
                    }
                }
            }
            BalanceDto balance = _api.TransferUpAsync(_currentUser.UserId, keywordArray[1], amount).GetAwaiter().GetResult();
            if(balance==null)
            {
                Console.WriteLine("Transfer Failed");
            }
            else
            {
                Console.WriteLine("Transferred " + keywordArray[2] + " to " + keywordArray[1]);
                ShowBalance(balance);
            }
        }

        private bool IsUserLoggedIn()
        {
            if(_currentUser==null)
            {
                Console.WriteLine("Please login before doing any transactions");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckForDebtTansfer(BalanceDto balance, decimal amount)
        {
            if (balance.DebtToList != null && balance.DebtToList.Count > 0)
            {
                foreach (DebtToDto debtTo in balance.DebtToList)
                {
                   
                    if (debtTo.DebtToAmount >= amount)
                    {
                        string[] keyWordsArr = new string[] { "pay", debtTo.DebtToUserName, amount.ToString() };
                        Transfer(keyWordsArr, true);
                        return true;
                    }
                    else
                    {
                        //decimal transAmount = amount - debtTo.DebtToAmount;
                        string[] keyWordsArr = new string[] { "pay", debtTo.DebtToUserName, debtTo.DebtToAmount.ToString() };
                        Transfer(keyWordsArr, true);
                        return true;
                    }
                }
            }

            return false;
        }
        private void ShowBalance(BalanceDto balance)
        {
            Console.WriteLine("Your balance is " + balance.Amount + ".");

            if (balance.DebtFromList!=null && balance.DebtFromList.Count>0)
            {
                foreach (DebtFromDto debtFrom in balance.DebtFromList)
                {
                    if (debtFrom.DebtFromAmount > 0)
                    {
                        Console.WriteLine("Owing " + debtFrom.DebtFromAmount + " from " + debtFrom.DebtFromUserName + ".");
                    }
                }
            }

            if (balance.DebtToList != null && balance.DebtToList.Count > 0)
            {
                foreach (DebtToDto debtTo in balance.DebtToList)
                {
                    if (debtTo.DebtToAmount > 0)
                    {
                        Console.WriteLine("Owing " + debtTo.DebtToAmount + " to " + debtTo.DebtToUserName + ".");
                    }
                }
            }
        }

        private void ShowSaliutation()
        {
            Console.WriteLine("Hello, "+_currentUser.Username+"!");
        }

    }
}
