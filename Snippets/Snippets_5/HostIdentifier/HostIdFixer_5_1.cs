namespace Snippets5.HostIdentifier
{
    using System;
    using NServiceBus;

    class HostIdFixer_5_1
    {
        HostIdFixer_5_1(BusConfiguration busConfiguration)
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
    
