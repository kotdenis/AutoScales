using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutoScales.Server.Healthcheck
{
    public class LivenessCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Healthy"));
        }
    }
}
