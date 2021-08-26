namespace V2.Receiver
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    public class DoSomethingMoreHandler : IHandleMessages<DoSomethingMore>
    {
        static ILog log = LogManager.GetLogger<DoSomethingMoreHandler>();

        public Task Handle(DoSomethingMore message, IMessageHandlerContext context)
        {
            log.Info($"Hi, I did something for you based on your v2 data: {message}");
            return Task.CompletedTask;
        }
    }
}