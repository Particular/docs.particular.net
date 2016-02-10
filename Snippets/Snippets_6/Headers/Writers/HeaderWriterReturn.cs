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
    public class HeaderWriterReturn
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReturnV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration config = new EndpointConfiguration();
            config.EndpointName(endpointName);
            Type[] callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(callbackTypes);
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
               await context.Reply(100);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (context.IsMessageOfTye<MessageToSend>())
                {
                    string sendingText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(context.Headers);
                    SnippetLogger.Write(text: sendingText, suffix: "Sending", version: "6");
                }
                else
                {
                    string returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(context.Headers);
                    SnippetLogger.Write(text: returnText, suffix: "Returning", version: "6");
                    ManualResetEvent.Set();
                }
                return Task.FromResult(0);
            }
        }
    }
}