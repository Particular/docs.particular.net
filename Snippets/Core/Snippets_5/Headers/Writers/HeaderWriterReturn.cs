namespace Core5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterReturn
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterReturnV5";

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
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(typeof(ConfigErrorQueue));
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
                bus.Return(100);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    string sendingText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(transportMessage.Headers);
                    SnippetLogger.Write(text: sendingText, suffix: "Sending", version: "5");
                }
                else
                {
                    string returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(transportMessage.Headers);
                    SnippetLogger.Write(text: returnText, suffix: "Returning", version: "5");
                    ManualResetEvent.Set();
                }

            }

        }
    }
}