using Data.Persistence;
using Data.Repositories;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

            // PersonRepository injections
            services.AddScoped<IRepository<PersonEntity, Guid>, PersonRepository>();
            services.AddScoped<ICodeRepository<PersonEntity>, PersonRepository>();

            // VisitRepository injections
            services.AddScoped<IRepository<VisitEntity, Guid>, VisitRepository>();
            services.AddScoped<IVisitRepository<VisitEntity>, VisitRepository>();
            return services;
        }
    }
}
