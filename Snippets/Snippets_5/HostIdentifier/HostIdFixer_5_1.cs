namespace Snippets5.HostIdentifier
{
    using System;
    using NServiceBus;

    public class HostIdFixer_5_1
    {
        public void Start()
        {
            #region HostIdFixer 5.1

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UniquelyIdentifyRunningInstance()
                .UsingNames("endpointName", Environment.MachineName);
            // or
            Guid hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            busConfiguration.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(hostId);
            
            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
}
    
