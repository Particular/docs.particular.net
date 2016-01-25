namespace Snippets6.DataBus.Custom
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region PluginCustomDataBusV5 5

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
