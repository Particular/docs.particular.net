using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IJobHost, ContinuousJob>();
            services.AddSingleton(services);
        });

        return hostBuilder;
    }
}