namespace Core;

using System;
using System.Net;
using NServiceBus;

public class FQDNTest
{
    void FQDN(EndpointConfiguration endpointConfiguration)
    {
        #region MachineNameActionOverride

        endpointConfiguration.UniquelyIdentifyRunningInstance()
            .UsingHostName(Dns.GetHostEntry(Environment.MachineName).HostName);

        #endregion
    }
}