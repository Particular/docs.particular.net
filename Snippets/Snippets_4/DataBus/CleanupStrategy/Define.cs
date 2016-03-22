namespace Snippets4.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            string databusPath = string.Empty;

            #region DefineFileLocationForDatabusFiles
            Configure.With()
                .FileShareDataBus(@"c:\database_files\")
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            #endregion
        }
    }
}
