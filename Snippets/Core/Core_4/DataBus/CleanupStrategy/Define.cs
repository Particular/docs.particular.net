namespace Snippets4.DataBus.CleanupStrategy
{
    using NServiceBus;

    public class Define
    {
        public Define()
        {
            string databusPath = string.Empty;

            #region DefineFileLocationForDatabusFiles
            var configure = Configure.With();
            configure.FileShareDataBus(@"\\share\databus_attachments\");
            #endregion
        }
    }
}