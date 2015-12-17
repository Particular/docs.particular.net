namespace Snippets5.Headers.Writers
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
    public class HeaderWriterPublish
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        static string EndpointName = "HeaderWriterPublishV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(EndpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(config).Start())
            {
                //give time for the subscription to happen
                Thread.Sleep(3000);
                bus.Publish(new MessageToPublish());
                ManualResetEvent.WaitOne();
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
                    Endpoint = EndpointName + "@" + Environment.MachineName
                });
                return unicastBusConfig;
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(transportMessage.Headers);
                SnippetLogger.Write(headerText, version: "All");
                ManualResetEvent.Set();
            }
        }
    }
}