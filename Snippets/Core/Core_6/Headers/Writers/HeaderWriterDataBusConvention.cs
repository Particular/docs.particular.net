﻿namespace Core6.Headers.Writers
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterDataBusConvention
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusConventionV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(@"..\..\..\storage");
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusConvention>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.StartsWith("LargeProperty");
            });
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            var messageToSend = new MessageToSend
            {
                LargeProperty1 = new byte[10],
                LargeProperty2 = new byte[10]
            };
            await endpointInstance.SendLocal(messageToSend)
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        class MessageToSend :
            IMessage
        {
            public byte[] LargeProperty1 { get; set; }
            public byte[] LargeProperty2 { get; set; }
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterDataBusConvention>(context.Headers)
                    .Replace(typeof(MessageToSend).FullName, "MessageToSend");
                SnippetLogger.Write(headerText);
                SnippetLogger.Write(Encoding.Default.GetString(context.Body),
                    suffix: "Body");
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}