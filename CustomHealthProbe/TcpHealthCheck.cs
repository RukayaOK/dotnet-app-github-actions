using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomHealthProbe;

    public sealed class TcpHealthProbeService : BackgroundService
    {private readonly HealthCheckService _healthCheckService;
        private readonly TcpListener _listener;
        private readonly ILogger<TcpHealthProbeService> _logger;
        private readonly IConfiguration _config; // makes available to all methods


        public TcpHealthProbeService(
            HealthCheckService healthCheckService,
            ILogger<TcpHealthProbeService> logger,
            IConfiguration config)
        {
            _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
            _logger = logger;
            _config = config;

            // Attach TCP listener to the port in configuration
            var port = _config.GetValue<int?>("HealthProbe:TcpPort") ?? 5009;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Started health check service.");
            await Task.Yield();
            _listener.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                // Gather health metrics every second.
                await UpdateHeartbeatAsync(stoppingToken);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            _listener.Stop();
        }

        private async Task UpdateHeartbeatAsync(CancellationToken token)
        {
            try
            {
                // Get health check results
                var result = await _healthCheckService.CheckHealthAsync(token);
                var isHealthy = result.Status == HealthStatus.Healthy;

                // Based on the response that we receive, we either halt the TCP listener and do not report the service's health
                if (!isHealthy)
                {
                    _listener.Stop();
                    _logger.LogInformation("Service is unhealthy. Listener stopped.");
                    return;
                }

                // respond to the probe 
                _listener.Start();
                while (_listener.Server.IsBound && _listener.Pending())
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    client.GetStream().Close();
                    client.Close();
                    _logger.LogInformation("Successfully processed health check request.");
                }

                _logger.LogDebug("Heartbeat check executed.");
            }
            #pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while checking heartbeat.");
            }
            #pragma warning restore CA1031 // Do not catch general exception types
        }
    }


