using DemoLibrary.Infraestructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

// register all repo related injections here

namespace DemoLibrary.CrossCutting
{
    public static class RepoInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            return services;
        }
    }
}