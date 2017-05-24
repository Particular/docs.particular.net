namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    class SpecifyingHostId
    {
        SpecifyingHostId(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-Specifying-HostId-Using-Api
            endpointConfiguration.UniquelyIdentifyRunningInstance()
                .UsingNames("endpointName", Environment.MachineName);
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