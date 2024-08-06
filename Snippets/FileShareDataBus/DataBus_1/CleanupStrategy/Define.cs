namespace CleanupStrategy
{
    using NServiceBus.ClaimCheck.DataBus;

    class Define
    {
        Define(NServiceBus.EndpointConfiguration endpointConfiguration)
        {
            #region DefineFileLocationForDatabusFiles

            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            dataBus.BasePath(@"\\share\databus_attachments\");

            #endregion
        }
    }
}