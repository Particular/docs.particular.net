namespace Snippets6.HostIdentifier
{
    using System;
    using NServiceBus;

    public class HostIdFixer
    {
        public void Start()
        {
            #region HostIdFixer

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UniquelyIdentifyRunningInstance()
                .UsingNames("endpointName", Environment.MachineName);
            // or
            Guid hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            endpointConfiguration.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(hostId);
            
            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
}
    
