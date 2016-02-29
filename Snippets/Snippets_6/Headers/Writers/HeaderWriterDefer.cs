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
    public class HeaderWriterDefer
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static bool Received;
        static string EndpointName = "HeaderWriterDeferV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(EndpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EndpointName(EndpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            SendOptions options = new SendOptions();
            options.DelayDeliveryWith(TimeSpan.FromMilliseconds(10));
            await endpoint.Send(new MessageToSend(),options);
            ManualResetEvent.WaitOne();
            await endpoint.Stop();
        }

        class MessageToSend : IMessage
        {
        }
    
        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
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
                    Endpoint = EndpointName +"@" + Environment.MachineName
                });
                return unicastBusConfig;
            }
        }
        class Mutator : IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterDefer>(context.Headers);
                SnippetLogger.Write(headerText, version: "6");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}