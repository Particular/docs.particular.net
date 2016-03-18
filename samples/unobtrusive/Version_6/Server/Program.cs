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
        Console.Title = "Samples.Unobtrusive.Server";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Server");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseDataBus<FileShareDataBus>()
            .BasePath(@"..\..\..\DataBusShare\");
        byte[] encryptionKey = Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        endpointConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.ApplyCustomConventions();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await CommandSender.Start(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}

