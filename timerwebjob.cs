using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .ConfigureServices(services =>
            {
                services.AddHostedService<MyWebJob>();
            })
            .Build();

        await host.RunAsync();
    }
}

public class MyWebJob : BackgroundService
{
    private readonly ILogger<MyWebJob> _logger;

    public MyWebJob(ILogger<MyWebJob> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"WebJob running at: {DateTime.Now}");

            await Task.Delay(5000);
        }
    }
}
