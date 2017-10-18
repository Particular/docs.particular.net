namespace Shipping
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SimulationEffects
    {
        TimeSpan baseProcessingTime = TimeSpan.FromMilliseconds(500);
        TimeSpan increment = TimeSpan.FromMilliseconds(500);

        public void WriteState(TextWriter output)
        {
            output.WriteLine("Base time to handle each OrderBilled event: {0} seconds", baseProcessingTime.TotalSeconds);
        }

        public Task SimulateMessageProcessing()
        {
            return Task.Delay(baseProcessingTime);
        }

        public void ProcessMessagesFaster()
        {
            if (baseProcessingTime > increment)
                baseProcessingTime -= increment;
        }

        public void ProcessMessagesSlower()
        {
            baseProcessingTime += increment;
        }
    }
}