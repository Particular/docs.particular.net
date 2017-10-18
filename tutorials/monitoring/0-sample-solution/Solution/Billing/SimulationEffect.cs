namespace Billing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SimulationEffects
    {
        bool networkLatencySimulationActive;
        double failureRate;
        const double failureRateIncrement = 0.1;
        Random r = new Random();

        public void ToggleNetworkLatencySimulation()
        {
            networkLatencySimulationActive = !networkLatencySimulationActive;
        }

        public void IncreaseFailureRate()
        {
            failureRate = Math.Min(1, failureRate + failureRateIncrement);
        }

        public void DecreaseFailureRate()
        {
            failureRate = Math.Max(0, failureRate - failureRateIncrement);
        }

        public void WriteState(TextWriter output)
        {
            output.Write("Network latency simulation: ");
            output.WriteLine(networkLatencySimulationActive ? "ON" : "OFF");

            output.WriteLine("Failure rate: {0:P}", failureRate);
        }

        public async Task SimulateNetworkLatency()
        {
            if (networkLatencySimulationActive)
            {
                await Task.Delay(TimeSpan.FromSeconds(2))
                    .ConfigureAwait(false);
            }
        }

        public Task SimulatedMessageProcessing()
        {
            if (r.NextDouble() < failureRate)
            {
                throw new Exception("BOOM! A failure occurred");
            }

            return Task.CompletedTask;
        }
    }
}
