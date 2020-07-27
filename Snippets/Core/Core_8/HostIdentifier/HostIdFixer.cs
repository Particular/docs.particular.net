namespace Core8.HostIdentifier
{
    using System;
    using NServiceBus;

    class HostIdFixer
    {
        HostIdFixer(EndpointConfiguration endpointConfiguration)
        {
            #region HostIdFixer

            endpointConfiguration.UniquelyIdentifyRunningInstance()
                .UsingNames(
                    instanceName: "endpointName",
                    hostName: Environment.MachineName);
            // or
            var hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
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