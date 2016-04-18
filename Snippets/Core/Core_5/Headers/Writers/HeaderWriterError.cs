namespace Core5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterError>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (UnicastBus bus = (UnicastBus) Bus.Create(busConfiguration).Start())
            {
                bus.Builder.Build<BusNotifications>()
                    .Errors
                    .MessageSentToErrorQueue
                    .Subscribe(e =>
                    {
                        string headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(e.Headers);
                        headerText = BehaviorCleaner.CleanStackTrace(headerText);
                        headerText = StackTraceCleaner.CleanStackTrace(headerText);
                        SnippetLogger.Write(text: headerText, suffix: "Error", version: "5");
                        ManualResetEvent.Set();
                    });
                bus.SendLocal(new MessageToSend());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
        }

        class TransportConfigProvider : IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 1
                };
            }
        }

        class ConfigureSecondLevelRetries : IProvideConfiguration<SecondLevelRetriesConfig>
        {
            public SecondLevelRetriesConfig GetConfiguration()
            {
                return new SecondLevelRetriesConfig
                {
                    Enabled = false,
                };
            }
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
                throw new Exception("The exception message from the handler.");
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            static bool hasCapturedMessage = false;
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (!hasCapturedMessage && transportMessage.IsMessageOfTye<MessageToSend>())
                {
                    hasCapturedMessage = true;
                    string sendingText = HeaderWriter.ToFriendlyString<HeaderWriterError>(transportMessage.Headers);
                    SnippetLogger.Write(text: sendingText, suffix: "Sending", version: "5");
                }
            }
        }
    }
}