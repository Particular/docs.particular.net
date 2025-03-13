using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

partial class Program
{
    public class MyScheduledTaskHandler(ILogger<MyScheduledTaskHandler> logger) : IHandleMessages<MyScheduledTask>
    {
        public Task Handle(MyScheduledTask message, IMessageHandlerContext context)
        {
            logger.LogInformation(nameof(MyScheduledTask) + " invoked");
            return Task.CompletedTask;
        }
    }
}