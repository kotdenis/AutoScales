using AutoScales.Core.Services.Implementation;
using AutoScales.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AutoScales.Core.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IWeighingService, WeighingService>();
            services.AddScoped<IJournalService, JournalService>();
        }
    }
}
