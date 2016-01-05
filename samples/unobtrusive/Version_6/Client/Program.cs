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
        busConfiguration.EndpointName("Samples.Unobtrusive.Client");
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
            IBusSession busSession = endpoint.CreateBusSession();
            await CommandSender.Start(busSession);
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}

