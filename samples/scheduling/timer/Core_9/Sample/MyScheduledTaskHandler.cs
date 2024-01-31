using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

partial class Program
{
    public class MyScheduledTaskHandler : IHandleMessages<MyScheduledTask>
    {
        static ILog log = LogManager.GetLogger<MyHandler>();

        public Task Handle(MyScheduledTask message, IMessageHandlerContext context)
        {
            log.Info(nameof(MyScheduledTask) + " invoked");
            return Task.CompletedTask;
        }
    }
}