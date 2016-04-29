namespace Snippets6.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            #region DefineFileLocationForDatabusFiles
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("endpointName");
            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(@"\\share\databus_attachments\");

            #endregion
        }
    }
}