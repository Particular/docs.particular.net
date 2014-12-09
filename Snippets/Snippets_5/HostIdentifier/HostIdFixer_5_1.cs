using System;
using NServiceBus;

public class HostIdFixer_5_1
{
    public void Start()
    {
        #region HostIdFixer 5.1

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
    
