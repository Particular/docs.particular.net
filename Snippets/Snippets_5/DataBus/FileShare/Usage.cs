namespace Snippets5.DataBus.FileShare
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration, string databusPath)
        {
            #region FileShareDataBus

            busConfiguration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}