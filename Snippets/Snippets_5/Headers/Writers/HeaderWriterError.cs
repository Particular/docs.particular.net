using System;
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

    static ManualResetEvent ManualResetEvent;
    string endpointName = "HeaderWriterErrorV5";

    [SetUp]
    [TearDown]
    public void Setup()
    {
        QueueCreation.DeleteQueuesForEndpoint(endpointName);
    }

    [Test]
    public void Write()
    {
        ManualResetEvent = new ManualResetEvent(false);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName(endpointName);
        busConfiguration.TypesToScan(TypeScanner.TypesFor<HeaderWriterError>());
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
        using (IStartableBus startableBus = Bus.Create(busConfiguration))
        using (UnicastBus bus = (UnicastBus) startableBus.Start())
        {
            bus.Builder.Build<BusNotifications>()
                .Errors
                .MessageSentToErrorQueue
                .Subscribe(e =>
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(e.Headers);
                    headerText = BehaviorCleaner.CleanStackTrace(headerText);
                    headerText = StackTraceCleaner.CleanStackTrace(headerText);
                    SnippetLogger.Write(text: headerText, suffix: "Error");
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
                Enabled = true,
                NumberOfRetries = 1,
                TimeIncrease = TimeSpan.FromMilliseconds(1)
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
                SnippetLogger.Write(text: sendingText, suffix: "Sending");
            }
        }
    }
}