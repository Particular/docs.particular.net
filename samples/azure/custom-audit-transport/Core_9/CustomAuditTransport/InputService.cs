using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class InputService(IMessageSession messageSession, IHostApplicationLifetime appLifetime) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press [s] to send a message. Press [Esc] to exit.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (Console.KeyAvailable)
                    {
                        var input = Console.ReadKey(intercept: true);

                        switch (input.Key)
                        {
                            case ConsoleKey.S:
                                await SendAuditMessageAsync(messageSession, stoppingToken);
                                break;
                            case ConsoleKey.Escape:
                                Console.WriteLine("\nExiting...");
                                appLifetime.StopApplication();
                                return;
                        }
                    }
                    else
                    {
                        await Task.Delay(20, stoppingToken); // More responsive polling
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
            }
        }

        private static async Task SendAuditMessageAsync(IMessageSession messageSession, CancellationToken cancellationToken)
        {
            var auditThisMessage = new AuditThisMessage
            {
                Content = $"{DateTime.UtcNow.ToShortTimeString()} - see you in the audit queue!"
            };
            await messageSession.SendLocal(auditThisMessage, cancellationToken);
            Console.WriteLine("\nMessage sent to local endpoint for auditing.");
        }
    }


