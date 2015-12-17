namespace Snippets5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterAudit
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        const string endpointName = "HeaderWriterAuditV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(config).Start())
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
            public void Handle(MessageToSend message)
            {
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            static bool receivedFirstMessage;

            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (!receivedFirstMessage)
                {
                    receivedFirstMessage = true;
                    string sendText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(transportMessage.Headers);
                    SnippetLogger.Write(text: sendText, suffix: "Send",version:"All");
                    return;
                }
                string auditText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(transportMessage.Headers);
                SnippetLogger.Write(text: auditText, suffix: "Audit", version: "All");
                ManualResetEvent.Set();
            }
        }

        class ProvideAuditConfig : IProvideConfiguration<AuditConfig>
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