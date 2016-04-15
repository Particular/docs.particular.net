namespace Snippets5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterReply
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReplyV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new MessageToSend());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            IBus bus;

            public MessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MessageToSend message)
            {
                bus.Reply(new MessageToReply());
            }
        }

        class MessageToReply : IMessage
        {
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (transportMessage.IsMessageOfTye<MessageToReply>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Replying", version: "5");
                    ManualResetEvent.Set();
                }
                if (transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Sending", version: "5");
                }
            }
        }
    }
}