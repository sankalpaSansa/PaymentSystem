using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Entities
{
    public class EntityDBContex : DbContext
    {
        public EntityDBContex()
        {
        }

        public EntityDBContex(DbContextOptions<EntityDBContex> options)
       : base(options)
        {
        }

        /* Define a DbSet for each entity of the application */
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<DebtFromStatus> DebtFromStatuses { get; set; }
        public DbSet<DebtToStatus> DebtToStatuses { get; set; }

    }
}
