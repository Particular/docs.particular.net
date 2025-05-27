using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace CustomAuditTransport
{
    public class InputService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var auditThisMessage = new AuditThisMessage
            {
                Content = "See you in the audit queue!"
            };
            await messageSession.SendLocal(auditThisMessage);

            Console.WriteLine("Message sent to local endpoint for auditing. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
