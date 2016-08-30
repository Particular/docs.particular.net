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
    public class HeaderWriterDefer
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static bool Received;
        static string EndpointName = "HeaderWriterDeferV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletionUtils.DeleteQueue(EndpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                bus.Defer(TimeSpan.FromMilliseconds(10),new MessageToSend());
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend :
            IMessage
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
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
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterDefer>(transportMessage.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
            }
        }
    }
}