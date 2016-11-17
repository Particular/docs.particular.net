using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NServiceBus;

public class Program
{

    static void Main()
    {
        JobHost host;
        string connectionString;
        // To run webjobs locally, can't use storage emulator with v1.
        // For local execution, use connection string stored in environment variable
        if ((connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString")) != null)
        {
            var configuration = new JobHostConfiguration
            {
                DashboardConnectionString = connectionString,
                StorageConnectionString = connectionString
            };
            host = new JobHost(configuration);
        }
        // for production, use DashboardConnectionString and StorageConnectionString defined at Azure website
        else
        {
            host = new JobHost();
        }

        Console.WriteLine("Starting VideoStore.Sales host");
        host.Call(typeof(Program).GetMethod(nameof(Program.AsyncMain)));
        host.RunAndBlock();
    }

    [NoAutomaticTrigger]
    public static async Task AsyncMain(CancellationToken cancellationToken)
    {
        Console.Title = "Samples.Store.ContentManagement";
        var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
        endpointConfiguration.ApplyCommonConfiguration();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(3000, cancellationToken)
                .ConfigureAwait(false);
        }
        
        await endpointInstance.Stop()
                .ConfigureAwait(false);
    }
}
