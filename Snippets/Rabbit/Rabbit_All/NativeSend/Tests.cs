using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Logging;
using NUnit.Framework;
using Rabbit_All.QueueDeletion;

namespace Rabbit_All.NativeSend
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        string endpointName = "rabbitNativeSendTests";
        static string errorQueueName = "rabbitNativeSendTestsError";

        static TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint("amqp://guest:guest@localhost:5672", endpointName);
            DeleteEndpointQueues.DeleteQueuesForEndpoint("amqp://guest:guest@localhost:5672", errorQueueName);
        }

        [Test]
        public async Task Send()
        {
            var endpointInstance = await StartBus()
                .ConfigureAwait(false);
            var headers = new Dictionary<string, object>
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
            var logFactory = LogManager.Use<DefaultFactory>();
            logFactory.Level(LogLevel.Warn);
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.SendFailedMessagesTo(errorQueueName);
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: setting =>
                {
                    setting.NumberOfRetries(0);
                });
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            var rabbitTypes = typeof(RabbitMQTransport).Assembly.GetTypes();
            var typesToScan = TypeScanner.NestedTypes<Tests>(rabbitTypes);
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            return Endpoint.Start(endpointConfiguration);
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                Assert.AreEqual("Value", message.Property);
                tcs.SetResult(true);
                return Task.CompletedTask;
            }
        }

        class MessageToSend :
            IMessage
        {
            public string Property { get; set; }
        }
    }
}