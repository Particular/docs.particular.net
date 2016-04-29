namespace Core6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterAudit
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        const string endpointName = "HeaderWriterAuditV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo(endpointName);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            await endpoint.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
            await endpoint.Stop();
        }

        class MessageToSend : IMessage
        {
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            static bool receivedFirstMessage;

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (!receivedFirstMessage)
                {
                    receivedFirstMessage = true;
                    string sendText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(context.Headers);
                    SnippetLogger.Write(text: sendText, suffix: "Send", version: "6");
                    return Task.FromResult(0);
                }
                string auditText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(context.Headers);
                SnippetLogger.Write(text: auditText, suffix: "Audit", version: "6");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }

    }
}