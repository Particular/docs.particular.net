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
        static IBus Bus;
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
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterReply>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (Bus = NServiceBus.Bus.Create(config).Start())
            {
                Bus.SendLocal(new MessageToSend());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
                Bus.Reply(new MessageToReply());
                Bus.Return(100);
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
                    SnippetLogger.Write(text: headerText, suffix: "Replying", version: "All");
                    ManualResetEvent.Set();
                }
                if (transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Sending", version: "All");
                }
            }
        }
    }
}