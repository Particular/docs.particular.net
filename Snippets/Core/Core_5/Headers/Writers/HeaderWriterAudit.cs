namespace Core5.Headers.Writers
{
    using System.Threading;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterAudit
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        const string endpointName = "HeaderWriterAuditV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>(typeof(ConfigErrorQueue));
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
            static bool receivedFirstMessage;

            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (!receivedFirstMessage)
                {
                    receivedFirstMessage = true;
                    var sendText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(transportMessage.Headers);
                    SnippetLogger.Write(text: sendText, suffix: "Send");
                    return;
                }
                var auditText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(transportMessage.Headers);
                SnippetLogger.Write(text: auditText, suffix: "Audit");
                ManualResetEvent.Set();
            }
        }

        class ProvideAuditConfig :
            IProvideConfiguration<AuditConfig>
        {
            public AuditConfig GetConfiguration()
            {
                return new AuditConfig
                {
                    QueueName = endpointName,
                };
            }
        }
    }
}