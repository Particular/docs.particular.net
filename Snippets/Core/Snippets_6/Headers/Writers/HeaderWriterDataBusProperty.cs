namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterDataBusProperty
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusPropertyV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.UseDataBus<FileShareDataBus>().BasePath(@"..\..\..\storage");
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusProperty>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            await endpoint.SendLocal(new MessageToSend
            {
                LargeProperty1 = new DataBusProperty<byte[]>(new byte[10]),
                LargeProperty2 = new DataBusProperty<byte[]>(new byte[10])
            });
            ManualResetEvent.WaitOne();
            await endpoint.Stop();
        }

        class MessageToSend : IMessage
        {
            public DataBusProperty<byte[]> LargeProperty1 { get; set; }
            public DataBusProperty<byte[]> LargeProperty2 { get; set; }
        }
    
        class MessageHandler : IHandleMessages<MessageToSend>
        { 
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterDataBusProperty>(context.Headers);
                SnippetLogger.Write(headerText, version: "6");
                SnippetLogger.Write(Encoding.Default.GetString(context.Body), version: "6", suffix: "Body");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}