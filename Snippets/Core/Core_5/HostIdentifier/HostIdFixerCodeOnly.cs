namespace Core5.HostIdentifier
{
    using System;
    using NServiceBus;

    class HostIdFixerCodeOnly
    {
        HostIdFixerCodeOnly(BusConfiguration busConfiguration)
        {
            #region HostIdFixer 5.1

            HostInfoSettings hostInfoSettings = busConfiguration.UniquelyIdentifyRunningInstance();
            hostInfoSettings.UsingNames("endpointName", Environment.MachineName);
            // or
            Guid hostId = CreateMyUniqueIdThatIsTheSameAcrossRestarts();
            hostInfoSettings.UsingCustomIdentifier(hostId);

            #endregion
        }

        Guid CreateMyUniqueIdThatIsTheSameAcrossRestarts()
        {
            throw new NotImplementedException();
        }
    }
}

