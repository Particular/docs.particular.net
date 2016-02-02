namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterReply
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReplyV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            Type[] callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(callbackTypes);
            config.SetTypesToScan(typesToScan);
            config.SendFailedMessagesTo("error");
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            IEndpointInstance endpoint = await Endpoint.Start(config);
            await endpoint.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
        }

        class MessageToSend : IMessage
        {
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public async Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                await context.Reply(new MessageToReply());
            }
        }

        class MessageToReply : IMessage
        {
        }

        class Mutator : IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (context.IsMessageOfTye<MessageToReply>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Replying", version: "6");
                    ManualResetEvent.Set();
                }
                if (context.IsMessageOfTye<MessageToSend>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Sending", version: "6");
                }
                return Task.FromResult(0);
            }
        }
    }
}