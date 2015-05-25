using System;
using System.Collections.Generic;
using System.Threading;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.MessageMutator;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterDefer
{
    public static ManualResetEvent ManualResetEvent;
    public static bool Received;
    static string EndpointName = "HeaderWriterDeferV5";

    [SetUp]
    [TearDown]
    public void Setup()
    {
        QueueCreation.DeleteQueuesForEndpoint(EndpointName);
    }

    [Test]
    public void Write()
    {
        ManualResetEvent = new ManualResetEvent(false);
        BusConfiguration config = new BusConfiguration();
        config.EndpointName(EndpointName);
        IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>(typeof(ConfigErrorQueue));
        config.TypesToScan(typesToScan);
        config.EnableInstallers();
        config.UsePersistence<InMemoryPersistence>();
        config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
        using (IStartableBus startableBus = Bus.Create(config))
        using (IBus bus = startableBus.Start())
        {
            bus.Defer(TimeSpan.FromMilliseconds(10),new MessageToSend());
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
                Endpoint = EndpointName +"@" + Environment.MachineName
            });
            return unicastBusConfig;
        }
    }
    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers);
            SnippetLogger.Write(headerText);
            ManualResetEvent.Set();
        }
    }
}