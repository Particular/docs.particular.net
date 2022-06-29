﻿using System;
using System.Diagnostics;
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
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
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
        });

        return hostBuilder;
    }

    #region WebJobHost_CriticalError
    static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage =
            $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";
        EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

        try
        {
            await context.Stop().ConfigureAwait(false);
        }
        finally
        {
            Environment.FailFast(fatalMessage, context.Exception);
        }
    }
    #endregion
}