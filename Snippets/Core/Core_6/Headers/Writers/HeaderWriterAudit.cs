namespace Core6.Headers.Writers
{
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
            QueueDeletionUtils.DeleteQueue(endpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo(endpointName);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });

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
                return Task.FromResult(0);
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
                    return Task.FromResult(0);
                }
                var auditText = HeaderWriter.ToFriendlyString<HeaderWriterAudit>(context.Headers);
                SnippetLogger.Write(
                    text: auditText,
                    suffix: "Audit");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }

    }
}