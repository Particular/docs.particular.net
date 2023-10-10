using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration, string databusPath)
    {
        #region FileShareDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(databusPath);

        #endregion
    }
}
