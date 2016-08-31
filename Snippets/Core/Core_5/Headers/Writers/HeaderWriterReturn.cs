﻿namespace Core5.Headers.Writers
{
    using System.Threading;
    using Common;
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
            QueueDeletionUtils.DeleteQueue(endpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var messageToSend = new MessageToSend();
                bus.SendLocal(messageToSend);
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend :
            IMessage
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
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

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                var headers = transportMessage.Headers;
                if (transportMessage.IsMessageOfTye<MessageToSend>())
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
            }

        }
    }
}