namespace Snippets5.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            #region DefineFileLocationForDatabusFiles
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseDataBus<FileShareDataBus>().BasePath(@"c:\database_files\");
            #endregion
        }
    }
}
