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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Unobtrusive.Server");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseDataBus<FileShareDataBus>()
            .BasePath(@"..\..\..\DataBusShare\");
        busConfiguration.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh8qCaDaxJXl+e7");
        busConfiguration.SendFailedMessagesTo("error");

        busConfiguration.ApplyCustomConventions();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
            await CommandSender.Start(busContext);
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}

