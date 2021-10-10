using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using System;

namespace PaymentSystem.Repo.Tests
{
    [TestClass]
    public class UserRepoTest : RepoTestBase
    {
        public UserRepoTest(): base(
            new DbContextOptionsBuilder<EntityDBContex>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {

        }

        UserDto testUser = new UserDto();
        [TestMethod]
        public void TestGetUser()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var userRepo = new UserRepo(context);

                var testUser = userRepo.GetUser("Alice");
                Assert.AreEqual(testUser.Username, "Alice");
               
            }
        }

        [TestMethod]
        public void TestCreateUser()
        {

            using (var context = new EntityDBContex(ContextOptions))
            {
                var userRepo = new UserRepo(context);

                testUser = userRepo.CreateUser("Bob");
                Assert.AreEqual(testUser.Username, "Bob");

            }
        }
    }
}
