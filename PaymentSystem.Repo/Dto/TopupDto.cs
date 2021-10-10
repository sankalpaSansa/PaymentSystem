using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Dto
{
    public class TopupDto
    {
        public Guid UserId { get; set; }
        public decimal TopupAmount { get; set; }
    }
}
