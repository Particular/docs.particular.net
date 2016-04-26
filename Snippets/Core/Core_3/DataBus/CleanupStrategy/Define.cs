namespace Snippets3.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            #region DefineFileLocationForDatabusFiles
            Configure.With()
                .FileShareDataBus(@"\\share\databus_attachments\")
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            #endregion
        }
    }
}