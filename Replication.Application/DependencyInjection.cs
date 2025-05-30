using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }

    }
}
