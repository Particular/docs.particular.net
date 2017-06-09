namespace Snippets6.Transports.MSMQ
{
    using System;
    using System.Net;
    using NServiceBus.Support;

    public class FQDMTest
    {
        void FQDM()
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