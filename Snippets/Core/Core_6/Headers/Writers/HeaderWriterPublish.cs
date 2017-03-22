#pragma warning disable 618
namespace Core6.Headers.Writers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterPublish
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        static string EndpointName = "HeaderWriterPublishV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            // give time for the subscription to happen
            await Task.Delay(3000)
                .ConfigureAwait(false);
            await endpointInstance.Publish(new MessageToPublish())
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        class MessageToPublish :
            IEvent
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToPublish>
        {
            public Task Handle(MessageToPublish message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
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
                    Endpoint = EndpointName
                };
                unicastBusConfig.MessageEndpointMappings.Add(endpointMapping);
                return unicastBusConfig;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(context.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}