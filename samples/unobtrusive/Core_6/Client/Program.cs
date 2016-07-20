using System;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Unobtrusive.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath(@"..\..\..\DataBusShare\");
        var encryptionKey = Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        endpointConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await CommandSender.Start(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}

