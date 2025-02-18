using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

public class Toggle : BackgroundService
{
    private readonly ILogger<Toggle> logger;

    public Toggle(ILogger<Toggle> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Press 't' to toggle fault mode.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var key = await ConoleHelper.ReadKeyAsync(stoppingToken);
                if (key == ConsoleKey.T)
                {
                    SimpleMessageHandler.FaultMode = !SimpleMessageHandler.FaultMode;
                    Console.WriteLine();
                    Console.WriteLine("Fault mode " + (SimpleMessageHandler.FaultMode ? "enabled" : "disabled"));
                }
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}