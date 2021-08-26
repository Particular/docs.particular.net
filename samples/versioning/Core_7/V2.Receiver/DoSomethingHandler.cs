namespace V2.Receiver
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        static ILog log = LogManager.GetLogger<DoSomethingMoreHandler>();

        public Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            log.Info("Received a v1 message and missing data, do what's needed to retrieve that data");

            context.Publish<DoSomethingMore>(v2 =>
            {
                v2.SomeData = message.SomeData;
                v2.SomeMoreData = 5; // set this value with the retrieved data
            }).ConfigureAwait(false);

            return Task.CompletedTask;
        }
    }
}