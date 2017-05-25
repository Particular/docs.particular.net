namespace Core6.CleanupStrategy
{
    using NServiceBus;

    class Define
    {
        Define(EndpointConfiguration endpointConfiguration)
        {
            #region DefineFileLocationForDatabusFiles

            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(@"\\share\databus_attachments\");

            #endregion
        }
    }
}