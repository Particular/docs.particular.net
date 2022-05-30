using NServiceBus;
using NServiceBus.DataBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region BinaryDataBusUsage
#pragma warning disable CS0618 // Type or member is obsolete
        endpointConfiguration.UseDataBus<FileShareDataBus, BinaryFormatterDataBusSerializer>();
#pragma warning restore CS0618 // Type or member is obsolete
        #endregion
    }
}
