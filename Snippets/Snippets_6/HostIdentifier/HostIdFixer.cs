namespace Snippets6.HostIdentifier
{
    using System;
    using NServiceBus;

    public class HostIdFixer
    {
        public void Start()
        {
            #region HostIdFixer

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
    
