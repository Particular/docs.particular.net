using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

class Program
{
    static IHostApplicationLifetime lifetime;

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        host.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        #region generic-host-service-lifetime

        var builder = Host.CreateDefaultBuilder(args);
        builder.UseWindowsService();

        #endregion

        /*
        #region generic-host-console-lifetime

        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        #endregion
        */

        #region generic-host-logging

        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            logging.AddEventLog();
            logging.AddConsole();
        });

        #endregion

        #region generic-host-nservicebus

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            return endpointConfiguration;
        });

        #endregion

        #region generic-host-worker-registration

        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });

        #endregion
    }

    #region generic-host-critical-error

    static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
    {
        // Not needed to call await context.Stop(cancellationToken) when invoking lifetime.StopApplication();
        lifetime.StopApplication();
    }

    #endregion
}