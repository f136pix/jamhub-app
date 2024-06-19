using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.Context;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

// register all repo related injections here

namespace DemoLibrary.CrossCutting
{
    public static class InfraestructureInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<IConfirmationTokenRepository, ConfirmationTokenRepository>();
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}