using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Dto
{
    public class TransferDto
    {
        public Guid Id { get; set; }
        public string PayeeUserName { get; set; }
        public decimal TransferAmount { get; set; }
    }
}
