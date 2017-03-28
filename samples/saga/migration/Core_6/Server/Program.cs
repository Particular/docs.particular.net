using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SagaMigration.Server";

        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True;");
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}