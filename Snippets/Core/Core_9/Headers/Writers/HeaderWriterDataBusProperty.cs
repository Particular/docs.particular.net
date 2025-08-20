namespace Core.Headers.Writers
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

#pragma warning disable CS0618 // Type or member is obsolete

    [TestFixture]
    public class HeaderWriterDataBusProperty
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusProperty";

        [OneTimeTearDown]
        public void TearDown()
        {
            ManualResetEvent.Dispose();
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            dataBus.BasePath(@"..\..\..\storage");
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusProperty>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.UseTransport(new LearningTransport {StorageDirectory = TestContext.CurrentContext.TestDirectory});
            endpointConfiguration.RegisterMessageMutator(new Mutator());
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            var messageToSend = new MessageToSend
            {
                LargeProperty1 = new DataBusProperty<byte[]>(new byte[10]),
                LargeProperty2 = new DataBusProperty<byte[]>(new byte[10])
            };
            await endpointInstance.SendLocal(messageToSend);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop();
        }

        class MessageToSend :
            IMessage
        {
            public DataBusProperty<byte[]> LargeProperty1 { get; set; }
            public DataBusProperty<byte[]> LargeProperty2 { get; set; }
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
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterDataBusProperty>(context.Headers);
                SnippetLogger.Write(headerText);
                SnippetLogger.Write(Encoding.Default.GetString(context.Body.ToArray()), suffix: "Body");
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}