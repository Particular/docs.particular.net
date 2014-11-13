namespace Snippets_4.HostIdentifier
{
    using System;
    using NServiceBus;

    public class HostIdFixer
    {
        public void Start()
        {
            #region HostIdFixer-V5

            var config = new BusConfiguration();
            config.UniquelyIdentifyRunningInstance()
                  .UsingNames("endpointName", Environment.MachineName);
            // or
            var hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            config.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(hostId);
            
            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
    
}
