using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Sales
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales");
            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(3);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(3));
                });

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}