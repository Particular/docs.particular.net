namespace Core8.Headers.Writers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterEncryption
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterEncryptionV8";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var encryptionService = new RijndaelEncryptionService(
                encryptionKeyIdentifier: "2015-10",
                key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));

            endpointConfiguration.EnableMessagePropertyEncryption(
                encryptionService: encryptionService,
                encryptedPropertyConvention: propertyInfo =>
                {
                    return propertyInfo.Name.EndsWith("EncryptedProperty");
                }
            );

            var conventions = endpointConfiguration.Conventions();
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterEncryption>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.RegisterMessageMutator(new Mutator());

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            var messageToSend = new MessageToSend
            {
                EncryptedProperty1 = "String 1",
                EncryptedProperty2 = "String 2"
            };
            await endpointInstance.SendLocal(messageToSend)
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
        }

        class MessageToSend :
            IMessage
        {
            public string EncryptedProperty1 { get; set; }
            public string EncryptedProperty2 { get; set; }
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
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterEncryption>(context.Headers);
                SnippetLogger.Write(headerText);
                SnippetLogger.Write(Encoding.Default.GetString(context.Body),
                    suffix: "Body");
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}