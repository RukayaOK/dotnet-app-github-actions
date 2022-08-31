using SimpleWorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CustomHealthProbe;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Health check services. A custom health check service is added for demo.
        services.AddHealthChecks().AddCheck<CustomHealthCheck>("custom_hc");

        services.AddHostedService<Worker>();
        services.AddHostedService<TcpHealthProbeService>();

        //services.AddSingleton<IHostedService, TcpHealthProbeService>();
    })
    .Build();

await host.RunAsync();

