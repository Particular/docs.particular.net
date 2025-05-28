using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace CustomAuditTransport
{
    public class InputService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press [s] to send a message. Press [Esc] to exit.");
            while (true)
            {
                var input = Console.ReadKey();
                Console.WriteLine();

                switch (input.Key)
                {
                    case ConsoleKey.S:
                        var auditThisMessage = new AuditThisMessage
                        {
                            Content = "See you in the audit queue!"
                        };
                        await messageSession.SendLocal(auditThisMessage, stoppingToken);
                        Console.WriteLine("Messages sent to local endpoint for auditing. Press any key to exit...");
                        break;                    
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
