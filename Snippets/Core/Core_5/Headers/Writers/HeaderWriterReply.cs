namespace Core5.Headers.Writers
{
    using System.Threading;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterReply
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReplyV5";

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
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(typeof(ConfigErrorQueue));
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
            IBus bus;

            public MessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MessageToSend message)
            {
                var messageToReply = new MessageToReply();
                bus.Reply(messageToReply);
            }
        }

        class MessageToReply :
            IMessage
        {
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                var headers = transportMessage.Headers;
                if (transportMessage.IsMessageOfTye<MessageToReply>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Replying",
                        version: "5");
                    ManualResetEvent.Set();
                }
                if (transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Sending",
                        version: "5");
                }
            }
        }
    }
}