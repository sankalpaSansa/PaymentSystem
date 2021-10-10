using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Repo.Entities
{
    public class Wallet
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public long Id { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
