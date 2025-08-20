namespace Core.Headers.Writers
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

        const string endpointName = "HeaderWriterAudit";

        [OneTimeTearDown]
        public void TearDown()
        {
            ManualResetEvent.Dispose();
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterAudit>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.AuditProcessedMessagesTo(endpointName);
            endpointConfiguration.UseTransport(new LearningTransport {StorageDirectory = TestContext.CurrentContext.TestDirectory});
            endpointConfiguration.RegisterMessageMutator(new Mutator());
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            await endpointInstance.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop();
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