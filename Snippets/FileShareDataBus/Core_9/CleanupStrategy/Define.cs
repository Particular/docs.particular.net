namespace CleanupStrategy
{
    using NServiceBus;

    class Define
    {
        Define(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            #region DefineFileLocationForDatabusFiles

            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            dataBus.BasePath(@"\\share\databus_attachments\");

            #endregion
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}