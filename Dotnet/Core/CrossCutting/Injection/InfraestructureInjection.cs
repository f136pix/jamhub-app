using System.Runtime.Serialization;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Profiles;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.Context;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// register all repo related injections here

namespace DemoLibrary.CrossCutting
{
    public static class InfraestructureInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICommonRepository<Person>, PeopleRepository>();
            services.AddScoped<ICommonRepository<Band>, BandsRepository>();
            services.AddScoped<ITokenRepository<ConfirmationToken>, ConfirmationTokenRepository>();
            services.AddScoped<ITokenRepository<BlacklistedToken>, BlacklistRepository>();
            
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DBConnection")));

            return services;
        }
    }
}