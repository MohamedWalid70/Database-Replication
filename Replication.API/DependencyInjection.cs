using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Sagas.Exclusive;
using Replication.Application;
using Replication.Application.AnotherTransactions;
using Replication.Application.Transactions;

namespace Replication.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
        {
 

            services.AddRebus(rebusConfig =>

                rebusConfig.Routing(routerConfig =>
                        routerConfig.TypeBased().MapAssemblyOf<ApplicationAssemblyReference>(configuration["MessageQueueConfig:QueueName"])).
                    Transport(transportConfig =>
                        transportConfig.UseRabbitMq(configuration.GetConnectionString("MessageBroker"), configuration["MessageQueueConfig:QueueName"])).
                    Sagas(saga => {
                        saga.StoreInSqlServer(configuration.GetConnectionString("DefaultDb"), "Sagas", "Sagas_indexes");
                        })
                    ,

                    onCreated: async (bus) =>
                    {
                        await bus.Subscribe<TransactionDataReplicatedEvent>();
                        await bus.Subscribe<TransactionReplicationFailedEvent>();
                        await bus.Subscribe<AnotherTransactionDeleteReplicationFailed>();
                        await bus.Subscribe<AnotherTransactionReplicationSuccess>();
                        await bus.Subscribe<AnotherTransactionDeleteReplicationFailed>();
                    }

            );

            services.Scan(selector =>

                 selector.
                 FromAssemblies(Replication.Infrastructure.InfrastructureAssemblyReference.AssemblyReference).
                 AddClasses(false).
                 AsImplementedInterfaces().
                 WithScopedLifetime()
             );


            services.AutoRegisterHandlersFromAssemblyOf<ApplicationAssemblyReference>();

            return services;
        }

    }
}
