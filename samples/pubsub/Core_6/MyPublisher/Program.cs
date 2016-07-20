using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.PubSub.MyPublisher";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.MyPublisher");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region SubscriptionAuthorizer

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.SubscriptionAuthorizer(context =>
        {
            var subscriptionType = context.MessageHeaders[Headers.MessageIntent];
            var messageIntentEnum = (MessageIntentEnum) Enum.Parse(typeof(MessageIntentEnum), subscriptionType, true);
            if (messageIntentEnum == MessageIntentEnum.Unsubscribe)
            {
                return true;
            }

            var lowerEndpointName = context.MessageHeaders[Headers.SubscriberEndpoint]
                .ToLowerInvariant();
            return lowerEndpointName.StartsWith("samples.pubsub.subscriber1") ||
                   lowerEndpointName.StartsWith("samples.pubsub.subscriber2");
        });

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await Start(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to publish IEvent");
        Console.WriteLine("Press '2' to publish EventMessage");
        Console.WriteLine("Press '3' to publish AnotherEventMessage");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var eventId = Guid.NewGuid();
            var time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
            var duration = TimeSpan.FromSeconds(99999D);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    await endpointInstance.Publish<IMyEvent>(m =>
                        {
                            m.EventId = eventId;
                            m.Time = time;
                            m.Duration = duration;
                        })
                        .ConfigureAwait(false);
                    Console.WriteLine($"Published IMyEvent with Id {eventId}.");
                    continue;
                case ConsoleKey.D2:
                    var eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = time,
                        Duration = duration
                    };
                    await endpointInstance.Publish(eventMessage)
                        .ConfigureAwait(false);
                    Console.WriteLine($"Published EventMessage with Id {eventId}.");
                    continue;
                case ConsoleKey.D3:
                    var anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = time,
                        Duration = duration
                    };
                    await endpointInstance.Publish(anotherEventMessage)
                        .ConfigureAwait(false);
                    Console.WriteLine($"Published AnotherEventMessage with Id {eventId}.");
                    continue;
                default:
                    return;
            }
        }

        #endregion
    }
}