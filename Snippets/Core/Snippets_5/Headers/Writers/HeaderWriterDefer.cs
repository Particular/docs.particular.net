namespace Core5.Headers.Writers
{
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
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static bool Received;
        static string EndpointName = "HeaderWriterDeferV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(EndpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(busConfiguration).Start())
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
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterDefer>(transportMessage.Headers);
                SnippetLogger.Write(headerText, version: "5");
                ManualResetEvent.Set();
            }
        }
    }
}