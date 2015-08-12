namespace Operations.Msmq
{
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class NativeSendTests
    {
        string endpointName = "msmqNativeSendTests";
        static string errorQueueName = "msmqNativeSendTestsError";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
            QueueDeletion.DeleteQueue(errorQueueName);
        }

        [Test]
        public void Send()
        {
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                List<NativeSend.HeaderInfo> headers = new List<NativeSend.HeaderInfo>
                {
                    new NativeSend.HeaderInfo
                    {
                        Key = "NServiceBus.EnclosedMessageTypes",
                        Value = "Operations.Msmq.NativeSendTests+MessageToSend"
                    }
                };
                NativeSend.SendMessage(@".\private$\" + endpointName, @"{""Property"": ""Value"",}", headers);
                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            BusConfiguration config = new BusConfiguration();
            config.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
            config.EndpointName(endpointName);
            config.UseSerialization<JsonSerializer>();
            config.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>());
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.DisableFeature<SecondLevelRetries>();
            return Bus.Create(config).Start();
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