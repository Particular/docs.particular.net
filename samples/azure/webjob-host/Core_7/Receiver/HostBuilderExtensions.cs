﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureHost(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureHostConfiguration(builder =>
        {
            builder.AddEnvironmentVariables("DOTNET_");
            builder.AddEnvironmentVariables();
        });

        hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName.ToLowerInvariant()}.json", optional: true);
        });

        hostBuilder.ConfigureWebJobs(builder => { builder.AddAzureStorageCoreServices(); });

        hostBuilder.ConfigureLogging((context, builder) => { builder.AddConsole(); });

        #region WebJobHost_Start
        hostBuilder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("receiver");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();

            var transportConnectionString = ctx.Configuration.GetConnectionString("TransportConnectionString");

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString(transportConnectionString);

            return endpointConfiguration;
        });
        #endregion

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<SimulateWorkHostedService>();
            services.AddSingleton<IJobHost, NoOpJobHost>();
            services.AddSingleton(services);
        });

        return hostBuilder;
    }

    #region WebJobHost_CriticalError
    static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage =
            $"The following critical error was encountered:\n{context.Error}\nProcess is shutting down.";
        Logger.Fatal(fatalMessage, context.Exception);

        if (Environment.UserInteractive)
        {
            // so that user can see on their screen the problem
            await Task.Delay(10_000)
                .ConfigureAwait(false);
        }

        Environment.FailFast(fatalMessage, context.Exception);
    }
    #endregion

    static ILog Logger = LogManager.GetLogger(typeof(HostBuilderExtensions));
}