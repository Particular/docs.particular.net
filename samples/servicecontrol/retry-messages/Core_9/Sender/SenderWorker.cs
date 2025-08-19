using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System;

public class SenderWorker : BackgroundService
{
    private readonly IMessageSession messageSession;
    private readonly ILogger<SenderWorker> logger;

    public SenderWorker(IMessageSession messageSession, ILogger<SenderWorker> logger)
    {
        this.messageSession = messageSession;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var key = await ConoleHelper.ReadKeyAsync(stoppingToken);

                if (key != ConsoleKey.Enter)
                {
                    break;
                }

                var simpleMessage = new SimpleMessage();

                await messageSession.Send(simpleMessage);

                Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}