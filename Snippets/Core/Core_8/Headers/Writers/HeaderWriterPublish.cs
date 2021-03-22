namespace Core8.Headers.Writers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterPublish
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        static string EndpointName = "HeaderWriterPublishV8";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.RegisterMessageMutator(new Mutator());
            var routing = endpointConfiguration.UseTransport(new LearningTransport());

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            // give time for the subscription to happen
            await Task.Delay(3000)
                .ConfigureAwait(false);
            await endpointInstance.Publish(new MessageToPublish())
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        class MessageToPublish :
            IEvent
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToPublish>
        {
            public Task Handle(MessageToPublish message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(context.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}