using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.Sites.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.Router.Sites.Client");


        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        #region ConfigureClient

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routingSettings = transport.Routing().ConnectToRouter("SiteA");
        routingSettings.RouteToEndpoint(typeof(Ping), "Samples.Router.Sites.Server");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        while (true)
        {
            Console.ReadLine();
            var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var message = new Ping();
            var options = new SendOptions();
            options.SendToSites("SiteB");
            options.SetMessageId(id);
            await endpointInstance.Send(message, options)
                .ConfigureAwait(false);
        }
    }
}