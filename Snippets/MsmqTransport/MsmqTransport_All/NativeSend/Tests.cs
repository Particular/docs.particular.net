namespace CoreAll.Msmq.NativeSend
{
    using System.Collections.Generic;
    using System.Threading;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class Tests
    {
        string endpointName = "msmqNativeSendTests";
        static string errorQueueName = "msmqNativeSendTestsError";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName);
            QueueDeletionUtils.DeleteQueue(errorQueueName);
        }

        [Test]
        public void Send()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                var headers = new List<NativeSend.HeaderInfo>
                {
                    new NativeSend.HeaderInfo
                    {
                        Key = "NServiceBus.EnclosedMessageTypes",
                        Value = "Operations.Msmq.NativeSendTests+MessageToSend"
                    }
                };
                NativeSend.SendMessage($@".\private$\{endpointName}", @"{""Property"": ""Value"",}", headers);
                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance);
                });
            busConfiguration.EndpointName(endpointName);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<Tests>());
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();
            return Bus.Create(busConfiguration).Start();
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            State state;

            public MessageHandler(State state)
            {
                this.state = state;
            }

            public void Handle(MessageToSend message)
            {
                Assert.That(message.Property, Is.EqualTo("Value"));
                state.ResetEvent.Set();
            }
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
        }

        class MessageToSend :
            IMessage
        {
            public string Property { get; set; }
        }
        class ConfigTransport :
            IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 0
                };
            }
        }

        class ConfigErrorQueue :
            IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
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