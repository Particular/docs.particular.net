namespace Core5.HostIdentifier
{
    using System;
    using NServiceBus;

    class HostIdFixerCodeOnly
    {
        HostIdFixerCodeOnly(BusConfiguration busConfiguration)
        {
            #region HostIdFixer 5.1

            var hostInfoSettings = busConfiguration.UniquelyIdentifyRunningInstance();
            hostInfoSettings.UsingNames(
                instanceName: "endpointName",
                hostName: Environment.MachineName);
            // or
            var hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            hostInfoSettings.UsingCustomIdentifier(hostId);

            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
}

