using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Headers.Writers;

using System.Threading;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.MessageMutator;
using NUnit.Framework;

[TestFixture]
public class HeaderWriterReturn
{
    static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
    string endpointName = "HeaderWriterReturn";

    [OneTimeTearDown]
    public void TearDown()
    {
        ManualResetEvent.Dispose();
    }

    [Test]
    public async Task Write()
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var callbackTypes = typeof(RequestResponseExtensions).Assembly.GetTypes();
        var typesToScan = TypeScanner.NestedTypes<HeaderWriterReturn>(callbackTypes);
        endpointConfiguration.SetTypesToScan(typesToScan);
        endpointConfiguration.EnableCallbacks();
        endpointConfiguration.MakeInstanceUniquelyAddressable("A");
        endpointConfiguration.UseTransport(new LearningTransport {StorageDirectory = TestContext.CurrentContext.TestDirectory});
        endpointConfiguration.RegisterMessageMutator(new Mutator());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await messageSession.SendLocal(new MessageToSend());

        ManualResetEvent.WaitOne();
        await host.StopAsync();
    }

    class MessageToSend :
        IMessage
    {
    }

    class MessageHandler :
        IHandleMessages<MessageToSend>
    {
        public Task Handle(MessageToSend message, IMessageHandlerContext context)
        {
            return context.Reply(100);
        }
    }

    class Mutator :
        IMutateIncomingTransportMessages
    {

        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var headers = context.Headers;
            if (context.IsMessageOfTye<MessageToSend>())
            {
                var sendingText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(headers);
                SnippetLogger.Write(
                    text: sendingText,
                    suffix: "Sending");
            }
            else
            {
                var returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(headers);
                SnippetLogger.Write(
                    text: returnText,
                    suffix: "Returning");
                ManualResetEvent.Set();
            }
            return Task.CompletedTask;
        }
    }
}