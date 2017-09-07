using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Startup
{
    public Startup(IHostingEnvironment env)
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        #region EndpointConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.SendOnly();

        #endregion

        #region Routing

        var routing = transport.Routing();
        routing.RouteToEndpoint(
            assembly: typeof(MyMessage).Assembly,
            destination: "Samples.ASPNETCore.Endpoint");

        #endregion

        #region EndpointStart

        var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        #endregion

        #region ServiceRegistration

        services.AddSingleton<IMessageSession>(endpointInstance);

        #endregion

        services.AddMvc();
    }


    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddDebug();
        app.UseMvc();
    }
}