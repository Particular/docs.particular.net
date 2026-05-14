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
public class HeaderWriterPublish
{
    static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

    static string EndpointName = "HeaderWriterPublish";

    [OneTimeTearDown]
    public void TearDown()
    {
        ManualResetEvent.Dispose();
    }

    [Test]
    public async Task Write()
    {
        var endpointConfiguration = new EndpointConfiguration(EndpointName);
        var typesToScan = TypeScanner.NestedTypes<HeaderWriterPublish>();
        endpointConfiguration.SetTypesToScan(typesToScan);
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.RegisterMessageMutator(new Mutator());
        endpointConfiguration.UseTransport(new LearningTransport {StorageDirectory = TestContext.CurrentContext.TestDirectory});
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();

        // give time for the subscription to happen
        await Task.Delay(3000);
        await messageSession.Publish(new MessageToPublish());
        ManualResetEvent.WaitOne();
        await host.StopAsync();
    }

    class MessageToPublish :
        IEvent
    {
    }

    class MessageHandler :
        IHandleMessages<MessageToPublish>
    {
        public Task Handle(MessageToPublish message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class Mutator :
        IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var headerText = HeaderWriter.ToFriendlyString<HeaderWriterPublish>(context.Headers);
            SnippetLogger.Write(headerText);
            ManualResetEvent.Set();
            return Task.CompletedTask;
        }
    }
}