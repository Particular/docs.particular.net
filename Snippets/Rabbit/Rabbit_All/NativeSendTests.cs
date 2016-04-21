namespace Rabbit_All
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint("amqp://admin:password@localhost:5672", endpointName);
            QueueDeletion.DeleteQueuesForEndpoint("amqp://admin:password@localhost:5672", errorQueueName);
        }

        [Test]
        public void Send()
        {
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                Dictionary<string, object> headers = new Dictionary<string, object>
                {
                    {"NServiceBus.EnclosedMessageTypes", "Operations.RabbitMQ.NativeSendTests+MessageToSend"}
                };
                NativeSend.SendMessage(Environment.MachineName, endpointName, "admin", "password", @"{""Property"": ""Value"",}", headers);
                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            LogManager.Use<DefaultFactory>()
                .Level(LogLevel.Warn);
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
            busConfiguration.EndpointName(endpointName);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionString("host=localhost");
            Type[] rabbitTypes = typeof(RabbitMQTransport).Assembly.GetTypes();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>(rabbitTypes));
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();
            return Bus.Create(busConfiguration).Start();
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            State state;

            public MessageHandler(State state)
            {
                this.state = state;
            }

            public void Handle(MessageToSend message)
            {
                Assert.AreEqual("Value", message.Property);
                state.ResetEvent.Set();
            }
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
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

        class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
        {
            public MessageForwardingInCaseOfFaultConfig GetConfiguration()
            {
                return new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = errorQueueName
                };
            }
        }
    }
}