namespace Core7
{
    using System;
    using System.Net;
    using NServiceBus.Support;

    public class FQDNTest
    {
        void FQDN()
        {
            #region MsmqMachineNameFQDN

            RuntimeEnvironment.MachineNameAction = () =>
            {
                return Dns.GetHostEntry(Environment.MachineName).HostName;
            };

            #endregion
        }
    }
}