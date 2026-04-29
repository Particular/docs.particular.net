using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Headers.Writers;

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

    string endpointName = "HeaderWriterEncryption";

    [OneTimeTearDown]
    public void TearDown()
    {
        ManualResetEvent.Dispose();
    }

    [Test]
    public async Task Write()
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var encryptionService = new AesEncryptionService(
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
        endpointConfiguration.UseTransport(new LearningTransport {StorageDirectory = TestContext.CurrentContext.TestDirectory});
        endpointConfiguration.RegisterMessageMutator(new Mutator());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();

        var messageToSend = new MessageToSend
        {
            EncryptedProperty1 = "String 1",
            EncryptedProperty2 = "String 2"
        };
        await messageSession.SendLocal(messageToSend);
        ManualResetEvent.WaitOne();
        await host.StopAsync();
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
            SnippetLogger.Write(Encoding.Default.GetString(context.Body.ToArray()),
                suffix: "Body");
            ManualResetEvent.Set();
            return Task.CompletedTask;
        }
    }
}