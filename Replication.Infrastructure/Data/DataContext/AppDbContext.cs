using Microsoft.EntityFrameworkCore;
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

            base.OnModelCreating(modelBuilder);
        }

    }
}
