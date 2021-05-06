namespace Core6
{
    using System;
    using System.Net;
    using NServiceBus.Support;

    public class FQDNTest
    {
        void FQDN()
        {
            #region MachineNameActionOverride

            RuntimeEnvironment.MachineNameAction = () => Dns.GetHostEntry(Environment.MachineName).HostName;

            #endregion
        }
    }
}