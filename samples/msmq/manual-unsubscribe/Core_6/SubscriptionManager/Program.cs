using Messages;
using NServiceBus;
using System.Threading.Tasks;

namespace SubscriptionManager
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("SubscriptionManager");
            endpointConfiguration.UseTransport<MsmqTransport>();

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            var unsubscribeMessage = new ManualUnsubscribe()
            {
                MessageTypeName = "Messages.SomethingHappened",
                MessageVersion = "1.0.0",
                SubscriberEndpoint = "Subscriber",
                SubscriberTransportAddress = "Subscriber@ANIMAL"
            };

            await endpointInstance.Send("Publisher", unsubscribeMessage)
                .ConfigureAwait(false); ;

            await endpointInstance.Stop()
                    .ConfigureAwait(false);
        }
    }
}
