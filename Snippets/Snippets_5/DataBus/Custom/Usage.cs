namespace Snippets5.DataBus.Custom
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region PluginCustomDataBus

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
