namespace Snippets6.HostIdentifier
{
    using System;
    using NServiceBus;

    public class HostIdFixer
    {
        public void Start()
        {
            #region HostIdFixer

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UniquelyIdentifyRunningInstance()
                .UsingNames("endpointName", Environment.MachineName);
            // or
            Guid hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            configuration.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(hostId);
            
            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
}
    
