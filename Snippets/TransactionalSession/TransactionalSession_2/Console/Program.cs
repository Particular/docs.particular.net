using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

internal class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Console");
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.UsePersistence<NonDurablePersistence>();

            #region enabling-transactional-session
            endpointConfiguration.EnableTransactionalSession();
            #endregion

            return endpointConfiguration;
        });


        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });
    }
}

