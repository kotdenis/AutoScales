using AutoScales.Data.Repositories.Implementation;
using AutoScales.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AutoScales.Data.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IWeighingRepository, WeighingRepository>();
            services.AddScoped<ITransportQuantityRepository, TransportQuantityRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
        }
    }
}
