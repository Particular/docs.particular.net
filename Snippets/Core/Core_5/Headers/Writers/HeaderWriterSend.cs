namespace Core5.Headers.Writers
{
    using System.Threading;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterSend
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterSendV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletionUtils.DeleteQueue(endpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterSend>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var messageToSend = new MessageToSend();
                bus.SendLocal(messageToSend);
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend :
            IMessage
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
            }
        }
    }
}