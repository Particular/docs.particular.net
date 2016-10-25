using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Exercise
{

    #region PlaceOrder

    namespace Messages.Commands
    {
        public class PlaceOrder : ICommand
        {
            public string OrderId { get; set; }
        }
    }

    #endregion

    class Program
    {
        static async Task AsyncMain()
        {
            EndpointConfiguration endpointConfiguration = null;

            #region AddRunLoopToAsyncMain

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance);

            await endpointInstance.Stop().ConfigureAwait(false);

            #endregion
        }

        static Task RunLoop(IEndpointInstance endpoint)
        {
            return Task.CompletedTask;
        }
    }
}