using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NServiceBus;
using NServiceBus.SqlServer.HttpPassthrough;
using NUnit.Framework;
using SampleEndpoint;
using SampleNamespace;

#region IntegrationTests

[TestFixture]
public class IntegrationTests
{
    static ManualResetEvent resetEvent = new ManualResetEvent(false);

    [Test]
    public async Task Integration()
    {
        await Installation.Run();
        var endpoint = await Program.StartEndpoint();

        await SubmitMultipartForm();

        if (!resetEvent.WaitOne(TimeSpan.FromSeconds(5)))
        {
            throw new Exception("OutgoingMessage not received");
        }

        await endpoint.Stop();
    }

    static async Task SubmitMultipartForm()
    {
        var hostBuilder = new WebHostBuilder();
        hostBuilder.UseStartup<Startup>();
        using (var server = new TestServer(hostBuilder))
        using (var client = server.CreateClient())
        {
            client.DefaultRequestHeaders.Referrer = new Uri("http://TheReferrer");
            await new ClientFormSender(client).Send(
                route: "/SendMessage",
                message: "{\"Property\": \"Value\"}",
                typeName: "SampleMessage",
                typeNamespace: "SampleNamespace",
                destination: "SampleEndpoint",
                attachments: new Dictionary<string, byte[]>
                {
                    {"fileName", Encoding.UTF8.GetBytes("fileContents")}
                });
        }
    }

    class Handler : IHandleMessages<SampleMessage>
    {
        public Task Handle(SampleMessage message, IMessageHandlerContext context)
        {
            resetEvent.Set();
            return Task.CompletedTask;
        }
    }
}

#endregion