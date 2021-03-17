namespace Core8.Headers.Writers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterReturn
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReturnV8";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(callbackTypes);
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableCallbacks();
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
                return context.Reply(100);
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headers = context.Headers;
                if (context.IsMessageOfTye<MessageToSend>())
                {
                    var sendingText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(headers);
                    SnippetLogger.Write(
                        text: sendingText,
                        suffix: "Sending");
                }
                else
                {
                    var returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(headers);
                    SnippetLogger.Write(
                        text: returnText,
                        suffix: "Returning");
                    ManualResetEvent.Set();
                }
                return Task.CompletedTask;
            }
        }
    }
}