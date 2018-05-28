using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NServiceBus.SqlServer.HttpPassthrough;

public class Usage
{
    async Task Foo(HttpClient httpClient)
    {
        #region ClientFormSender

        var clientFormSender = new ClientFormSender(httpClient);
        await clientFormSender.Send(
            route: "/SendMessage",
            message: "{\"Property\": \"Value\"}",
            typeName: "TheMessageType",
            typeNamespace: "TheMessageNamespace",
            destination: "TheDestination",
            attachments: new Dictionary<string, byte[]>
            {
                {"fileName", Encoding.UTF8.GetBytes("fileContents")}
            });

        #endregion
    }

    static async Task SubmitMultipartForm()
    {
        #region asptesthost

        var hostBuilder = new WebHostBuilder();
        hostBuilder.UseStartup<Startup>();
        using (var testServer = new TestServer(hostBuilder))
        using (var httpClient = testServer.CreateClient())
        {
            var clientFormSender = new ClientFormSender(httpClient);
            await clientFormSender.Send(
                route: "/SendMessage",
                message: "{\"Property\": \"Value\"}",
                typeName: "TheMessageType",
                typeNamespace: "TheMessageNamespace",
                destination: "TheDestination",
                attachments: new Dictionary<string, byte[]>
                {
                    {"fileName", Encoding.UTF8.GetBytes("fileContents")}
                });
        }

        #endregion
    }
}