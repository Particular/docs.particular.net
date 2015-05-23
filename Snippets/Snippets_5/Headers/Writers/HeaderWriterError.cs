using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterError
{

    static ManualResetEvent ManualResetEvent;
    string endpointName = "HeaderWriterErrorV5";

    [Test]
    public void Write()
    {
        QueueCreation.DeleteQueuesForEndpoint(endpointName);
        try
        {
            ManualResetEvent = new ManualResetEvent(false);
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            busConfiguration.TypesToScan(TypeScanner.TypesFor<HeaderWriterError>());
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IStartableBus startableBus = Bus.Create(busConfiguration))
            using (UnicastBus bus = (UnicastBus)startableBus.Start())
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
        finally
        {
            QueueCreation.DeleteQueuesForEndpoint(endpointName);
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
                MaxRetries = 0
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
        public void MutateIncoming(TransportMessage transportMessage)
        {
            string sendingText = HeaderWriter.ToFriendlyString<HeaderWriterError>(transportMessage.Headers);
            SnippetLogger.Write(text: sendingText, suffix: "Sending");
        }
    }
}