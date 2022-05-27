using NServiceBus;
using NServiceBus.DataBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region BinaryDataBusUsage
        endpointConfiguration.UseDataBus<FileShareDataBus, BinaryFormatterDataBusSerializer>();
        #endregion
    }
}
