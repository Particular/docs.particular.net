namespace Core8.Headers.Writers
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
        string endpointName = "HeaderWriterReplyV8";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(callbackTypes);
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.MakeInstanceUniquelyAddressable("A");
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.RegisterMessageMutator(new Mutator());

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            await endpointInstance.SendLocal(new MessageToSend())
                .ConfigureAwait(false);
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