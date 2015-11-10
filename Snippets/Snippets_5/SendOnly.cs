namespace Snippets5
{
    using NServiceBus;

    public class SendOnly
    {
        public void Simple()
        {
            #region SendOnly

            BusConfiguration busConfiguration = new BusConfiguration();
            ISendOnlyBus bus = Bus.CreateSendOnly(busConfiguration);

            #endregion
        }

    }
}