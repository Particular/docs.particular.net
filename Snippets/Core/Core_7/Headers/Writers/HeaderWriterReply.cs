namespace Core.Headers.Writers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterReply
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReply";

        [OneTimeTearDown]
        public void TearDown()
        {
            ManualResetEvent.Dispose();
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(callbackTypes);
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.MakeInstanceUniquelyAddressable("A");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.StorageDirectory(TestContext.CurrentContext.TestDirectory);
            endpointConfiguration.RegisterMessageMutator(new Mutator());

            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            await endpointInstance.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
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
                var messageToReply = new MessageToReply();
                return context.Reply(messageToReply);
            }
        }

        class MessageToReply :
            IMessage
        {
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headers = context.Headers;
                if (context.IsMessageOfTye<MessageToReply>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Replying");
                    ManualResetEvent.Set();
                }
                if (context.IsMessageOfTye<MessageToSend>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Sending");
                }
                return Task.CompletedTask;
            }
        }
    }
}