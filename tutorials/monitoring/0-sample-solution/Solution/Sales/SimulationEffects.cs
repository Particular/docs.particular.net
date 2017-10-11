namespace Sales
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SimulationEffects
    {
        TimeSpan baseProcessingTime = TimeSpan.FromMilliseconds(500);
        TimeSpan increment = TimeSpan.FromMilliseconds(500);
        DateTime? degradingResourceSimulationStarted;
        const int degradationRate = 5;

        public void WriteState(TextWriter output)
        {
            output.WriteLine("Base time to handle each order: {0} seconds", baseProcessingTime.TotalSeconds);
            output.Write("Simulated degrading resource: ");
            output.WriteLine(degradingResourceSimulationStarted.HasValue ? "ON" : "OFF");
        }

        public Task SimulateMessageProcessing()
        {
            return Task.Delay(baseProcessingTime + Degradation());
        }

        TimeSpan Degradation()
        {
            var timeSinceDegradationStarted = DateTime.UtcNow - (degradingResourceSimulationStarted ?? DateTime.MaxValue);
            if (timeSinceDegradationStarted < TimeSpan.Zero) return TimeSpan.Zero;
            return new TimeSpan(timeSinceDegradationStarted.Ticks / degradationRate);
        }

        public void ProcessMessagesFaster()
        {
            if(baseProcessingTime > increment)
                baseProcessingTime -= increment;
        }

        public void ProcessMessagesSlower()
        {
            baseProcessingTime += increment;
        }

        public void ToggleDegradationSimulation()
        {
            degradingResourceSimulationStarted = degradingResourceSimulationStarted.HasValue ? default(DateTime?) : DateTime.UtcNow;
        }
    }
}