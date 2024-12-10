namespace V1.Subscriber
{
    using System.Threading.Tasks;
    using Contracts;
    using NServiceBus;
    using NServiceBus.Logging;

    public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
    {
        static readonly ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

        public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
        {
            log.Info($"Something happened with some data '{message.SomeData}'");
            return Task.CompletedTask;
        }
    }
}