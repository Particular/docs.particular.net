using NServiceBus;
using NServiceBus.DataBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region BinaryDataBusUsage
        endpointConfiguration.UseDataBus<FileShareDataBus, BinaryFormatterDataBusSerializer>();
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
