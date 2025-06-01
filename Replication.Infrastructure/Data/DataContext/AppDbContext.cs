using Microsoft.EntityFrameworkCore;
using Replication.Domain.Entities.AnotherTransaction;
using Replication.Domain.Entities.AnotherTransactionReplication;
using Replication.Domain.Entities.Transaction;
using Replication.Domain.Entities.TransactionReplication;

namespace Replication.Infrastructure.Data.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InfrastructureTransaction>(builder => builder.ToTable("InfrastructureTransactions"));
            modelBuilder.Entity<TransactionReplication>(builder => builder.ToTable("TransactionReplications"));
            modelBuilder.Entity<AnotherTransaction>(builder => builder.ToTable("AnotherTransactions"));
            modelBuilder.Entity<AnotherTransactionReplication>(builder => builder.ToTable("AnotherTransactionReplications"));

            base.OnModelCreating(modelBuilder);
        }

    }
}
