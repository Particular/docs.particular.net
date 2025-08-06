using System;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NServiceBus;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureHost(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureWebJobs(builder => builder.AddAzureStorageCoreServices());

        hostBuilder.ConfigureLogging((context, builder) =>
        {
            builder.AddConsole();
        });

        #region WebJobHost_Start
        hostBuilder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("receiver");
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
            endpointConfiguration.UsePersistence<NonDurablePersistence>();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.EnableInstallers();

            var transportConnectionString = ctx.Configuration.GetConnectionString("TransportConnectionString");
            endpointConfiguration.UseTransport(new AzureStorageQueueTransport(transportConnectionString));

            return endpointConfiguration;
        });
        #endregion

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<SimulateWorkHostedService>();
            services.AddSingleton<IJobHost, NoOpJobHost>();
        });

        return hostBuilder;
    }

    #region WebJobHost_CriticalError
    static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
    {
        var fatalMessage =
            $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

        try
        {
            await context.Stop(cancellationToken);
        }
        finally
        {
            Environment.FailFast(fatalMessage, context.Exception);
        }
    }
    #endregion
}