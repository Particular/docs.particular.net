using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Exercise
{

    #region PlaceOrder

    namespace Messages
    {
        public class PlaceOrder :
            ICommand
        {
            public string OrderId { get; set; }
        }
    }

    #endregion

    class Program
    {
        static async Task Main(EndpointConfiguration endpointConfiguration)
        {
            #region AddRunLoopToMain

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance);

            await endpointInstance.Stop();

            #endregion
        }

        static Task RunLoop(IEndpointInstance endpoint)
        {
            return Task.CompletedTask;
        }
    }
}