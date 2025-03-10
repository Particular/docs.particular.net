using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace AuditFilter
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

            var doNotAuditThisMessage = new DoNotAuditThisMessage
            {
                Content = "Don't look for me!"
            };
            await messageSession.SendLocal(doNotAuditThisMessage);

        }
    }
}
