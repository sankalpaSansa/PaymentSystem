using Microsoft.EntityFrameworkCore;
using PaymentSystem.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Tests
{
    public class RepoTestBase
    {

        protected RepoTestBase(DbContextOptions<EntityDBContex> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        protected DbContextOptions<EntityDBContex> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new EntityDBContex(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user1 = new User
                {
                    UserId = Guid.Parse("ee26f298-2d5f-4ea1-9219-a5f1573e48e9"),
                    Username = "Alice"
                };

                var user2 = new User
                {
                    UserId = Guid.Parse("bbc8b783-6ecc-44a3-99b1-bbd830202720"),
                    Username = "Bob"
                };

                var wallet1 = new Wallet
                {
                    User = user1,
                    Amount = 0
                };

                var wallet2 = new Wallet
                {
                    User = user2,
                    Amount = 50
                };
                context.AddRange(user1,user2);
                context.AddRange(wallet1, wallet2);

                context.SaveChanges();
            }
        }

    }
}
