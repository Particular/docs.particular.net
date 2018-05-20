using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MyNamespace;
using NServiceBus;
using NServiceBus.Attachments.Sql;
using NServiceBus.Features;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    static ManualResetEvent resetEvent = new ManualResetEvent(false);

    static string connectionString = @"Data Source=.\SQLExpress;Database=SqlHttpPassThroughSample; Integrated Security=True;Max Pool Size=100;MultipleActiveResultSets=True";

    [Test]
    public async Task Integration()
    {
        SqlHelper.EnsureDatabaseExists(connectionString);
        using (var connection = await ConnectionHelpers.OpenConnection(connectionString))
        {
            var manager = new DeduplicationManager(connection, "Deduplication");
            await manager.Create();
            await Installer.CreateTable(connection, "MessageAttachments");
        }

        var endpoint = await StartEndpoint();

        await SubmitMultipartForm();

        if (!resetEvent.WaitOne(TimeSpan.FromSeconds(2)))
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
                typeName: "MyMessage",
                typeNamespace: "MyNamespace",
                destination: "Endpoint",
                attachments: new Dictionary<string, byte[]>
                {
                    {"fileName", Encoding.UTF8.GetBytes("fileContents")}
                });
        }
    }

    static Task<IEndpointInstance> StartEndpoint()
    {
        var endpointConfiguration = new EndpointConfiguration("Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.PurgeOnStartup(true);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
        endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
        endpointConfiguration.EnableAttachments(connectionString, TimeToKeep.Default);
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        return Endpoint.Start(endpointConfiguration);
    }

    class Handler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachment = context.Attachments();
            await incomingAttachment.GetBytes("fileName");
            Assert.AreEqual("Value", message.Property);
            resetEvent.Set();
        }
    }
}