namespace SimpleWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private HttpClient client;
    private readonly IConfiguration _config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        client = default!;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        client = new HttpClient();
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        client.Dispose();
        return base.StopAsync(cancellationToken);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await client.GetAsync(_config.GetValue<string>("WebsiteURL"));
        var pollInterval = _config.GetValue<int>("PollInterval");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (result.IsSuccessStatusCode)
            {
                _logger.LogInformation("The website is up. Status code {StatusCode}", result.StatusCode);
                _logger.LogInformation("My secret password is: {Password}", _config.GetValue<string>("SecretPassword"));
               // _logger.LogInformation("My secret password is: {Password}", Environment.GetEnvironmentVariable("ClientId"));

            }
            else
            {
                // send email that website is down 
                _logger.LogError("The website is down. Status code {StatusCode}", result.StatusCode);
            }
            await Task.Delay(pollInterval, stoppingToken);
        }
    }
}

