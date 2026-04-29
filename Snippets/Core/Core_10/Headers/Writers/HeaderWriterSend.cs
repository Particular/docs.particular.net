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
public class HeaderWriterSend
{
    static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

    string endpointName = "HeaderWriterSend";

    [OneTimeTearDown]
    public void TearDown()
    {
        ManualResetEvent.Dispose();
    }

    [Test]
    public async Task Write()
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var typesToScan = TypeScanner.NestedTypes<HeaderWriterSend>();
        endpointConfiguration.SetTypesToScan(typesToScan);
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
            return Task.CompletedTask;
        }
    }

    class Mutator :
        IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(context.Headers);
            SnippetLogger.Write(headerText);
            ManualResetEvent.Set();
            return Task.CompletedTask;
        }
    }
}