namespace Core6.Headers.Writers
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
    public class HeaderWriterDataBusConvention
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusConventionV6";

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
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusConvention>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.Conventions().DefiningDataBusPropertiesAs(x => x.Name.StartsWith("LargeProperty"));
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            
            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            await endpoint.SendLocal(new MessageToSend
            {
                LargeProperty1 = new byte[10],
                LargeProperty2 = new byte[10]
            });
            ManualResetEvent.WaitOne();
            await endpoint.Stop();
        }

        class MessageToSend : IMessage
        {
            public byte[] LargeProperty1 { get; set; }
            public byte[] LargeProperty2 { get; set; }
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
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterDataBusConvention>(context.Headers)
                    .Replace(typeof(MessageToSend).FullName, "MessageToSend");
                SnippetLogger.Write(headerText, version: "6");
                SnippetLogger.Write(Encoding.Default.GetString(context.Body), version: "6", suffix: "Body");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}