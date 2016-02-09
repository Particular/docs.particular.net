namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
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

        static string EndpointName = "HeaderWriterPublishV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration config = new EndpointConfiguration();
            config.EndpointName(EndpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>();
            config.SetTypesToScan(typesToScan);
            config.SendFailedMessagesTo("error");
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            IEndpointInstance endpoint = await Endpoint.Start(config);

            //give time for the subscription to happen
            Thread.Sleep(3000);
            await endpoint.Publish(new MessageToPublish());
            ManualResetEvent.WaitOne();
            await endpoint.Stop();
        }

        class MessageToPublish : IEvent
        {
        }

        class MessageHandler : IHandleMessages<MessageToPublish>
        {
            public Task Handle(MessageToPublish message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
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
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(context.Headers);
                SnippetLogger.Write(headerText, version: "6");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}