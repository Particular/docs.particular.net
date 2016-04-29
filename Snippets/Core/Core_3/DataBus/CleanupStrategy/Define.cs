namespace Snippets3.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            #region DefineFileLocationForDatabusFiles
            var configure = Configure.With();
            configure.FileShareDataBus(@"\\share\databus_attachments\");
            #endregion
        }
    }
}