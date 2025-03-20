using System;
using System.Threading.Tasks;
using Endpoint1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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
                services.AddHostedService<InputLoopService>();

            }).UseNServiceBus(x =>
            {
                #region endpointName

                var endpointName = "Samples.Azure.StorageQueues.Endpoint1.With.A.Very.Long.Name.And.Invalid.Characters";
                var endpointConfiguration = new EndpointConfiguration(endpointName);

                #endregion

                Console.Title = endpointName;

                #region config

                var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true");
                var routingSettings = endpointConfiguration.UseTransport(transport);

                #endregion

                #region sanitization

                transport.QueueNameSanitizer = BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener;

                #endregion

                routingSettings.DisablePublishing();
                endpointConfiguration.UsePersistence<LearningPersistence>();
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                endpointConfiguration.EnableInstallers();


                return endpointConfiguration;
            });

}