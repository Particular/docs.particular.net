using NServiceBus;

class DataBusUpgradeGuide
{
    void ConfigureDataBusOld(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-DataBusUsage-UpgradeGuide-old
        endpointConfiguration.UseDataBus<FileShareDataBus>();
        #endregion
    }
}