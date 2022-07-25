namespace Receiver
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    public class MyEventHandler : IHandleMessages<MyEvent>
    {
        static ILog log = LogManager.GetLogger<MyEventHandler>();

        public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
        {
            log.Info($"Hello from {nameof(MyEventHandler)}");
            return Task.CompletedTask;
        }
    }
}