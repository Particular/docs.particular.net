namespace Core8.DataBus.Custom
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region PluginCustomDataBus
            endpointConfiguration.UseDataBus(svc => new CustomDataBus(), new SystemJsonDataBusSerializer());
            #endregion
        }
    }
}
