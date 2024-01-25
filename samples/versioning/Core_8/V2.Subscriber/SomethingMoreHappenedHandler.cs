namespace V2.Subscriber
{
    using System.Threading.Tasks;
    using Contracts;
    using NServiceBus;
    using NServiceBus.Logging;

    public class SomethingMoreHappenedHandler : IHandleMessages<ISomethingMoreHappened>
    {
        static readonly ILog log = LogManager.GetLogger<SomethingMoreHappenedHandler>();

        public Task Handle(ISomethingMoreHappened message, IMessageHandlerContext context)
        {
            log.Info($"Something happened with some data '{message.SomeData}' and more information '{message.MoreInfo}'");
            return Task.CompletedTask;
        }
    }
}