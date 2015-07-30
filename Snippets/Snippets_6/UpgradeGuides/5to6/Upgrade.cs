namespace Snippets6.UpgradeGuide
{
    using NServiceBus;

    public class Upgrade
    {
        void StaticHeaders()
        {

            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6header-static-endpoint
            busConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
