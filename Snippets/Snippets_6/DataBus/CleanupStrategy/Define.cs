namespace Snippets5.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            #region DefineFileLocationForDatabusFiles
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("endpointName");
            endpointConfiguration.UseDataBus<FileShareDataBus>().BasePath(@"c:\database_files\");

            #endregion
        }
    }
}
