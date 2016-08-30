﻿namespace Core5.Headers.Writers
{
    using System;
    using System.Threading;
    using Common;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterError
    {

        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterErrorV5";

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
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterError>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = (UnicastBus) Bus.Create(busConfiguration).Start())
            {
                bus.Builder.Build<BusNotifications>()
                    .Errors
                    .MessageSentToErrorQueue
                    .Subscribe(e =>
                    {
                        var headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(e.Headers);
                        headerText = BehaviorCleaner.CleanStackTrace(headerText);
                        headerText = StackTraceCleaner.CleanStackTrace(headerText);
                        SnippetLogger.Write(text: headerText, suffix: "Error");
                        ManualResetEvent.Set();
                    });
                var messageToSend = new MessageToSend();
                bus.SendLocal(messageToSend);
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend :
            IMessage
        {
        }

        class TransportConfigProvider :
            IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 1
                };
            }
        }

        class ConfigureDelayedRetries :
            IProvideConfiguration<SecondLevelRetriesConfig>
        {
            public SecondLevelRetriesConfig GetConfiguration()
            {
                return new SecondLevelRetriesConfig
                {
                    Enabled = false,
                };
            }
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
                throw new Exception("The exception message from the handler.");
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            static bool hasCapturedMessage;
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (!hasCapturedMessage && transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    hasCapturedMessage = true;
                    var headers = transportMessage.Headers;
                    var sendingText = HeaderWriter.ToFriendlyString<HeaderWriterError>(headers);
                    SnippetLogger.Write(
                        text: sendingText,
                        suffix: "Sending");
                }
            }
        }
    }
}