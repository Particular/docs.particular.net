namespace Core5.HostIdentifier
{
    using System;
    using NServiceBus;

    class HostIdFixerCodeOnly
    {
        HostIdFixerCodeOnly(BusConfiguration busConfiguration)
        {
            #region HostIdFixer 5.1

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

