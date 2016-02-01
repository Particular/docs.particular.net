namespace ThroughputThrottlingDemo
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.SendFailedMessagesTo("errorq");

            configuration.Pipeline.Register(
                "GitHub API Throttling", 
                typeof (ThrottlingBehavior),
                "implements API throttling for GitHub APIs");

            configuration.LimitMessageProcessingConcurrencyTo(1);
        }
    }
}
