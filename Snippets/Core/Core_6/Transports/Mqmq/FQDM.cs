namespace Snippets6.Transports.MSMQ
{
    using NServiceBus.Support;

    public class FQDMTest
    {
        void FQDM()
        {
            #region MsmqMachineNameFQDN [4,)

            RuntimeEnvironment.MachineNameAction = () => System.Net.Dns.GetHostEntry(Environment.MachineName).HostName;

            #endregion
        }       
    }
}