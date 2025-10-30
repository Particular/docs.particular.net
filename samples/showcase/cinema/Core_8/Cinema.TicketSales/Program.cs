using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Cinema.Messages;

namespace CinemaCounter
{
    internal static class Program
    {
        static string endpointName = "Cinema.TicketSales";
        private static string MonthId = DateTime.Now.ToString("yyyy-MM");

        public static async Task Main(string[] args)
        {
            if(args.Count() > 0) endpointName = args[0];

            Console.Title = endpointName;
            var host = CreateHostBuilder(args).Build();

            await host.StartAsync();

            var messageSession = host.Services.GetRequiredService<IMessageSession>();

            #region sales-desk
            Console.WriteLine("Press [Enter] to exit");
            Console.WriteLine("Press [B] to sell ticket for Barbie");
            Console.WriteLine("Press [O] to sell ticket for Oppenheimer");

            while (true)
            {
                if (!Console.KeyAvailable) continue;

                var userInput = Console.ReadKey();

                if (userInput.Key == ConsoleKey.Enter)
                {
                    break;
                }
                if (userInput.Key == ConsoleKey.B)
                {
                    await messageSession.Send(new RecordTicketSale
                    {
                        MonthId = MonthId,
                        FilmName = "Barbie"
                    }, CancellationToken.None);
                }
                else if (userInput.Key == ConsoleKey.O)
                {
                    await messageSession.Send(new RecordTicketSale
                    {
                        MonthId = MonthId,
                        FilmName = "Oppenheimer"
                    }, CancellationToken.None);
                }
            }
            #endregion

            await host.StopAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseNServiceBus(ctx =>
                {
                    // TODO: consider moving common endpoint configuration into a shared project
                    // for use by all endpoints in the system

                    var endpointConfiguration = new EndpointConfiguration(endpointName);

                    // Learning Transport: https://docs.net/transports/learning/
                    var routing = endpointConfiguration.UseTransport(new LearningTransport());

                    // Define routing for commands: https://docs.net/nservicebus/messaging/routing#command-routing
                     routing.RouteToEndpoint(typeof(RecordTicketSale), "Cinema.Headquarters");

                    // Learning Persistence: https://docs.net/persistence/learning/
                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    // Message serialization
                    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                    // Installers are useful in development. Consider disabling in production.
                    // https://docs.net/nservicebus/operations/installers
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                });
        }

        static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
        {
            // TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
            // https://docs.net/nservicebus/hosting/critical-errors
            // and consider setting up service recovery
            // https://docs.net/nservicebus/hosting/windows-service#installation-restart-recovery
            try
            {
                await context.Stop(cancellationToken);
            }
            finally
            {
                FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }

        static void FailFast(string message, Exception exception)
        {
            try
            {
                // TODO: decide what kind of last resort logging is necessary
                // TODO: when using an external logging framework it is important to flush any pending entries prior to calling FailFast
                // https://docs.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }
    }
}
