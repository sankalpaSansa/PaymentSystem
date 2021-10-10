using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using System;

namespace PaymentSystem.Repo.Tests
{
    [TestClass]
    public class PayRepoTest : RepoTestBase
    {
        public PayRepoTest() : base(
            new DbContextOptionsBuilder<EntityDBContex>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {

        }

        [TestMethod]
        public void GetBalance()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var payRepo = new PayRepo(context);

                var testTopup = payRepo.GetBalance(Guid.Parse("ee26f298-2d5f-4ea1-9219-a5f1573e48e9"));
                Assert.AreEqual(testTopup.Amount, 0);

            }
        }

        [TestMethod]
        public void FundTransfer()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var payRepo = new PayRepo(context);

                TransferDto transfer = new TransferDto
                {
                    Id = Guid.Parse("bbc8b783-6ecc-44a3-99b1-bbd830202720"),
                    TransferAmount = 10,
                    PayeeUserName = "Bob"
                };

                var testTopup = payRepo.FundTransfer(transfer);
                Assert.AreEqual(testTopup.Amount, 40);

            }
        }
    }
}
