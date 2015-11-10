namespace Snippets3
{
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    public class SendOnly
    {
        public void Simple()
        {
            #region SendOnly

            Configure configure = Configure.With();
            ConfigUnicastBus configUnicastBus = configure.UnicastBus();
            IBus bus = configUnicastBus.SendOnly();

            #endregion
        }
    }
}