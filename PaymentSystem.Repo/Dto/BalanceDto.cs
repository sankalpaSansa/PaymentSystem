using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Dto
{
    public class BalanceDto
    {
        public decimal Amount { get; set; }
        public List<DebtFromDto> DebtFromList { get; set; }
        public List<DebtToDto> DebtToList { get; set; }
    }

    public class DebtFromDto
    {
        public decimal DebtFromAmount { get; set; }
        public string DebtFromUserName { get; set; }
    }

    public class DebtToDto
    {
        public decimal DebtToAmount { get; set; }
        public string DebtToUserName { get; set; }
    }
}
