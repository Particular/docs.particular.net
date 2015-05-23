using System.Threading;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.MessageMutator;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterPublish
{
    public static ManualResetEvent ManualResetEvent;

    string endpointName = "HeaderWriterPublishV5";

    [Test]
    public void Write()
    {
        QueueCreation.DeleteQueuesForEndpoint(endpointName);
        try
        {
            ManualResetEvent = new ManualResetEvent(false);

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            busConfiguration.TypesToScan(TypeScanner.TypesFor<HeaderWriterPublish>());
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IStartableBus startableBus = Bus.Create(busConfiguration))
            using (IBus bus = startableBus.Start())
            {
                //give time for the subscription to happen
                Thread.Sleep(3000);
                bus.Publish(new MessageToPublish());
                ManualResetEvent.WaitOne();
            }
        }
        finally
        {
            QueueCreation.DeleteQueuesForEndpoint(endpointName);
        }
    }

    class MessageToPublish : IEvent
    {
    }

    class MessageHandler : IHandleMessages<MessageToPublish>
    {
        public void Handle(MessageToPublish message)
        {
        }
    }

    class ConfigUnicastBus : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            UnicastBusConfig unicastBusConfig = new UnicastBusConfig();
            unicastBusConfig.MessageEndpointMappings.Add(new MessageEndpointMapping
            {
                AssemblyName = GetType().Assembly.GetName().Name,
                Endpoint = "HeaderWriterPublishV5@retina"
            });
            return unicastBusConfig;
        }
    }

    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            string headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(transportMessage.Headers);
            SnippetLogger.Write(headerText);
            ManualResetEvent.Set();
        }
    }
}