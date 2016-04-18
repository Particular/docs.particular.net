namespace Core5.DataBus.Custom
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region PluginCustomDataBus

            busConfiguration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
