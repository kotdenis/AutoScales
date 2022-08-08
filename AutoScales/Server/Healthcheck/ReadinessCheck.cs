using AutoScales.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace AutoScales.Server.Healthcheck
{
    public class ReadinessCheck : IHealthCheck
    {
        private readonly ScalesDbContext _dbContext;

        public ReadinessCheck(ScalesDbContext scalesDbContext) => _dbContext = scalesDbContext;
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _ = await _dbContext.Database.ExecuteSqlInterpolatedAsync($"select 1;", cancellationToken);
            return HealthCheckResult.Healthy("Database is working");
        }
    }
}
