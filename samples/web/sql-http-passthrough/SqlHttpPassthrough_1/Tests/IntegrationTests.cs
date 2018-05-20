using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NServiceBus;
using NServiceBus.Attachments.Sql;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;
using NUnit.Framework;
using SampleEndpoint;
using SampleNamespace;

[TestFixture]
public class IntegrationTests
{
    static ManualResetEvent resetEvent = new ManualResetEvent(false);

    [Test]
    public async Task Integration()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString))
        {
            var manager = new DeduplicationManager(connection, "Deduplication");
            await manager.Create();
            await Installer.CreateTable(connection, "MessageAttachments");
        }

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
            var message = "{\"Property\": \"Value\"}";
            await ClientFormSender.Send(
                client,
                route: "/SendMessage",
                message: message,
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