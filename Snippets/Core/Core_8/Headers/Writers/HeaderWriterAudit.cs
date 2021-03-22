namespace Core8.Headers.Writers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterAudit
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        const string endpointName = "HeaderWriterAuditV8";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.AuditProcessedMessagesTo(endpointName);
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.RegisterMessageMutator(new Mutator());

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            await endpointInstance.SendLocal(new MessageToSend())
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        class MessageToSend :
            IMessage
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            static bool receivedFirstMessage;

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (!receivedFirstMessage)
                {
                    receivedFirstMessage = true;
                    var sendText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(context.Headers);
                    SnippetLogger.Write(
                        text: sendText,
                        suffix: "Send");
                    return Task.CompletedTask;
                }
                var auditText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(context.Headers);
                SnippetLogger.Write(
                    text: auditText,
                    suffix: "Audit");
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }

    }
}