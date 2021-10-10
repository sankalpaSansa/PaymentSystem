using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaymentSystem.Repo.Entities
{
    public class DebtToStatus
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public long Id { get; set; }
        public User User { get; set; }
        public User DebtTo { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
