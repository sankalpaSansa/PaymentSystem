using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using System;

namespace PaymentSystem.Repo.Tests
{
    [TestClass]
    public class TopupRepoTest : RepoTestBase
    {
        public TopupRepoTest() : base(
            new DbContextOptionsBuilder<EntityDBContex>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {

        }

        [TestMethod]
        public void TestTopup1()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var TopupRepo = new TopupRepo(context);

                TopupDto input = new TopupDto
                {
                    UserId = Guid.Parse("ee26f298-2d5f-4ea1-9219-a5f1573e48e9"),
                    TopupAmount = 100,
                };
                var testTopup = TopupRepo.TopupBalance(input);
                Assert.AreEqual(testTopup, 100);

            }
        }

        [TestMethod]
        public void TestTopup2()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var TopupRepo = new TopupRepo(context);

                TopupDto input = new TopupDto
                {
                    UserId = Guid.Parse("bbc8b783-6ecc-44a3-99b1-bbd830202720"),
                    TopupAmount = 100,
                };
                var testTopup = TopupRepo.TopupBalance(input);
                Assert.AreEqual(testTopup, 150);

            }
        }
    }
}
