using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NServiceBus;

public class Program
{
    static void Main()
    {
        var config = new JobHostConfiguration();
        if (config.IsDevelopment)
        {
            config.UseDevelopmentSettings();
        }
        var host = new JobHost(config);

        Console.WriteLine("Starting Store.Operations host");
        host.Call(typeof(Program).GetMethod(nameof(Program.AsyncMain)));
        host.RunAndBlock();
    }

    [NoAutomaticTrigger]
    public static async Task AsyncMain(CancellationToken cancellationToken)
    {
        Console.Title = "Samples.Store.Operations";
        var endpointConfiguration = new EndpointConfiguration("Store-Operations");
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