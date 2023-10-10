namespace CleanupStrategy
{
    using NServiceBus;

    class Define
    {
        Define(EndpointConfiguration endpointConfiguration)
        {
            #region DefineFileLocationForDatabusFiles

            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            dataBus.BasePath(@"\\share\databus_attachments\");

            #endregion
        }
    }
}