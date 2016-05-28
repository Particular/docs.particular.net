namespace Core5.Headers.Writers
{
    using System.Threading;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterReturn
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReturnV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new MessageToSend());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            IBus bus;

            public MessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MessageToSend message)
            {
                bus.Return(100);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    var sendingText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(transportMessage.Headers);
                    SnippetLogger.Write(text: sendingText, suffix: "Sending", version: "5");
                }
                else
                {
                    var returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(transportMessage.Headers);
                    SnippetLogger.Write(text: returnText, suffix: "Returning", version: "5");
                    ManualResetEvent.Set();
                }

            }

        }
    }
}