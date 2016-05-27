using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using NServiceBus.Logging;
using NUnit.Framework;

[TestFixture]
[Explicit]
public class NativeSendTests
{
    string endpointName = "rabbitNativeSendTests";
    static string errorQueueName = "rabbitNativeSendTestsError";

    static TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

    [SetUp]
    [TearDown]
    public void Setup()
    {
        QueueDeletion.DeleteQueuesForEndpoint("amqp://guest:guest@localhost:5672", endpointName);
        QueueDeletion.DeleteQueuesForEndpoint("amqp://guest:guest@localhost:5672", errorQueueName);
    }

    [Test]
    public async Task Send()
    {
        var endpointInstance = await StartBus()
             .ConfigureAwait(false);
        Dictionary<string, object> headers = new Dictionary<string, object>
        {
            {"NServiceBus.EnclosedMessageTypes", "NativeSendTests+MessageToSend"}
        };
        NativeSend.SendMessage("localhost", endpointName, "guest", "guest", @"{""Property"": ""Value"",}", headers);
        await tcs.Task.ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    Task<IEndpointInstance> StartBus()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.SendFailedMessagesTo(errorQueueName);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        Type[] rabbitTypes = typeof(RabbitMQTransport).Assembly.GetTypes();
        IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<NativeSendTests>(rabbitTypes);
        endpointConfiguration.SetTypesToScan(typesToScan);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        return Endpoint.Start(endpointConfiguration);
    }

    class MessageHandler : IHandleMessages<MessageToSend>
    {
        public Task Handle(MessageToSend message, IMessageHandlerContext context)
        {
            Assert.AreEqual("Value", message.Property);
            tcs.SetResult(true);
            return Task.FromResult(0);
        }
    }

    class MessageToSend : IMessage
    {
        public string Property { get; set; }
    }

    class ConfigTransport : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaxRetries = 0
            };
        }
    }

}
