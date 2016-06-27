namespace Operations.Msmq
{
    using System.Collections.Generic;
    using System.Threading;
    using Common;
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
            busConfiguration.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
            busConfiguration.EndpointName(endpointName);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>());
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