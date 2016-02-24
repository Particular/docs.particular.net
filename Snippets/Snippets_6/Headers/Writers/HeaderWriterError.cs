namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Faults;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterError
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterErrorV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EndpointName(endpointName);
            endpointConfiguration.SendFailedMessagesTo("error");
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterError>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            await endpoint.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
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
                    NumberOfRetries = 1,
                    TimeIncrease = TimeSpan.FromMilliseconds(1)
                };
            }
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public MessageHandler(BusNotifications busNotifications)
            {
                ErrorsNotifications errors = busNotifications.Errors;
                errors.MessageSentToErrorQueue += (sender, retry) =>
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(retry.Headers);
                    headerText = BehaviorCleaner.CleanStackTrace(headerText);
                    headerText = StackTraceCleaner.CleanStackTrace(headerText);
                    SnippetLogger.Write(text: headerText, suffix: "Error", version: "5");
                };
            }

            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                throw new Exception("The exception message from the handler.");
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            static bool hasCapturedMessage = false;

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (!hasCapturedMessage && context.IsMessageOfTye<MessageToSend>())
                {
                    hasCapturedMessage = true;
                    string sendingText = HeaderWriter.ToFriendlyString<HeaderWriterError>(context.Headers);
                    SnippetLogger.Write(text: sendingText, suffix: "Sending", version: "5");
                }
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}