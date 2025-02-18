using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration, string databusPath)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region FileShareDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(databusPath);

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
