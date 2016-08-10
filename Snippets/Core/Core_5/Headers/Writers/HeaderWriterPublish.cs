namespace Core5.Headers.Writers
{
    using System;
    using System.Threading;
    using Common;
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
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: c =>
                {
                    c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                // give time for the subscription to happen
                Thread.Sleep(3000);
                bus.Publish(new MessageToPublish());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToPublish :
            IEvent
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToPublish>
        {
            public void Handle(MessageToPublish message)
            {
            }
        }

        class ConfigUnicastBus :
            IProvideConfiguration<UnicastBusConfig>
        {
            public UnicastBusConfig GetConfiguration()
            {
                var unicastBusConfig = new UnicastBusConfig();
                var endpointMapping = new MessageEndpointMapping
                {
                    AssemblyName = GetType().Assembly.GetName().Name,
                    Endpoint = $"{EndpointName}@{Environment.MachineName}"
                };
                unicastBusConfig.MessageEndpointMappings.Add(endpointMapping);
                return unicastBusConfig;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(transportMessage.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
            }
        }
    }
}