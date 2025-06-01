using Microsoft.Extensions.DependencyInjection;
using Replication.Domain.Entities.AnotherTransaction;
using Replication.Domain.Entities.AnotherTransactionReplication;
using Replication.Domain.Entities.Transaction;
using Replication.Domain.Entities.TransactionReplication;

namespace Replication.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(ApplicationAssemblyReference.AssemblyReference));

            services.AddTransient<InfrastructureTransactionService>();
            services.AddTransient<TransactionReplicationService>();
            services.AddScoped<AnotherTransactionService>();
            services.AddScoped<AnotherTransactionReplicationService>();
            return services;
        }

    }
}
