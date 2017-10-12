namespace Billing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    class SimulationEffects
    {
        bool networkLatencySimulationActive;

        public void ToggleNetworkLatencySimulation()
        {
            networkLatencySimulationActive = !networkLatencySimulationActive;
        }

        public void WriteState(TextWriter output)
        {
            output.Write("Network latency simulation: ");
            output.WriteLine(networkLatencySimulationActive ? "ON" : "OFF");
        }

        public async Task SimulateNetworkLatency()
        {
            if (networkLatencySimulationActive)
            {
                await Task.Delay(TimeSpan.FromSeconds(2))
                    .ConfigureAwait(false);
            }
        }
    }
}
