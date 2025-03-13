using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "Server";
             services.AddHostedService<InputLoopService>();

         }).UseNServiceBus(x =>
         {
             Console.Title = "SimpleSaga";
             var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");

             #region config

             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();

             #endregion


             Console.WriteLine();
             Console.WriteLine("Storage locations:");
             Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
             Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

             Console.WriteLine();
             Console.WriteLine("Press 'Enter' to send a StartOrder message");
             return endpointConfiguration;
         });

}
