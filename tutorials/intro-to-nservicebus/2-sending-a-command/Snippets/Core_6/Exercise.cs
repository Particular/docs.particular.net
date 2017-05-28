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
        static async Task AsyncMain(EndpointConfiguration endpointConfiguration)
        {
            #region AddRunLoopToAsyncMain

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);

            #endregion
        }

        static Task RunLoop(IEndpointInstance endpoint)
        {
            return Task.CompletedTask;
        }
    }
}