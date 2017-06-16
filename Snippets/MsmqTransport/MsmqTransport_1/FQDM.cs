namespace Snippets6.Transports.MSMQ
{
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