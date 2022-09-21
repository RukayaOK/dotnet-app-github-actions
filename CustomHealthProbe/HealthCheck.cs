using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace CustomHealthProbe
{
    public class CustomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // requests all the registered health check services to report the health of the service
            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }
    }
}
