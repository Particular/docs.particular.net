using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System;

public class SenderWorker(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var key = await ConsoleHelper.ReadKeyAsync(stoppingToken);

                if (key != ConsoleKey.Enter)
                {
                    break;
                }

                var simpleMessage = new SimpleMessage();

                await messageSession.Send(simpleMessage, stoppingToken);

                Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}