namespace ClientUI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;

    class SimulatedCustomers
    {
        readonly IEndpointInstance endpointInstance;
        bool highTrafficMode;

        public SimulatedCustomers(IEndpointInstance endpointInstance)
        {
            this.endpointInstance = endpointInstance;
        }

        public void WriteState(TextWriter output)
        {
            var trafficMode = highTrafficMode ? "High" : "Low";
            output.WriteLine($"{trafficMode} traffic mode");
        }

        public void ToggleTrafficMode()
        {
            highTrafficMode = !highTrafficMode;
        }

        public async Task Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (highTrafficMode)
                {
                    await PlaceBatchOfOrders(200).ConfigureAwait(false);
                }
                else
                {
                    await PlaceSingleOrder().ConfigureAwait(false);
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        Task PlaceSingleOrder()
        {
            var placeOrderCommand = new PlaceOrder
            {
                OrderId = Guid.NewGuid().ToString()
            };

            return endpointInstance.Send(placeOrderCommand);
        }

        Task PlaceBatchOfOrders(int count)
        {
            return Task.WhenAll(
                Enumerable.Range(1, count).Select(x => PlaceSingleOrder())
            );
        }
    }
}