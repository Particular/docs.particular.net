namespace Core4.DataBus.CleanupStrategy
{
    using NServiceBus;

    class Define
    {
        Define()
        {
            #region DefineFileLocationForDatabusFiles
            var configure = Configure.With();
            configure.FileShareDataBus(@"\\share\databus_attachments\");
            #endregion
        }
    }
}