namespace Core6.Headers.Writers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterDefer
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static bool Received;
        static string EndpointName = "HeaderWriterDeferV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterMessageMutator(new Mutator());
            var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();
            routing.RouteToEndpoint(GetType().Assembly, EndpointName);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            var options = new SendOptions();
            options.DelayDeliveryWith(TimeSpan.FromMilliseconds(10));
            await endpointInstance.Send(new MessageToSend(),options)
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
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterDefer>(context.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}