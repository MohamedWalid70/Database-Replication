using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Replication.Application.Abstractions.Data;
using Replication.Domain.Repositories;
using Replication.Infrastructure.Data.DataContext;
using Replication.Infrastructure.Data.Repositories;
using Replication.Infrastructure.Data.UnitOfWork;

namespace Replication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultDb")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

    }
}
