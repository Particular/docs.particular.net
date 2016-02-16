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
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.PubSub.MyPublisher");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        #region SubscriptionAuthorizer
        endpointConfiguration.UseTransport<MsmqTransport>()
            .SubscriptionAuthorizer(context =>
            {
                string subscriptionType = context.MessageHeaders[Headers.MessageIntent];
                MessageIntentEnum messageIntentEnum = (MessageIntentEnum) Enum.Parse(typeof(MessageIntentEnum), subscriptionType, true);
                if (messageIntentEnum == MessageIntentEnum.Unsubscribe)
                {
                    return true;
                }

                string lowerEndpointName = context.MessageHeaders[Headers.SubscriberEndpoint]
                    .ToLowerInvariant();
                return lowerEndpointName.StartsWith("samples.pubsub.subscriber1") ||
                       lowerEndpointName.StartsWith("samples.pubsub.subscriber2");
            });
        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await Start(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to publish IEvent");
        Console.WriteLine("Press '2' to publish EventMessage");
        Console.WriteLine("Press '3' to publish AnotherEventMessage");
        Console.WriteLine("Press 'Enter' to publish a message.To exit, Ctrl + C");
        #region PublishLoop
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            Guid eventId = Guid.NewGuid();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    await endpointInstance.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
                        m.Duration = TimeSpan.FromSeconds(99999D);
                    });
                    Console.WriteLine("Published IMyEvent with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D2:
                    EventMessage eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    await endpointInstance.Publish(eventMessage);
                    Console.WriteLine("Published EventMessage with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D3:
                    AnotherEventMessage anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    await endpointInstance.Publish(anotherEventMessage);
                    Console.WriteLine("Published AnotherEventMessage with Id {0}.", eventId);
                    continue;
                default:
                    return;
            }
        }
        #endregion
    }
}

