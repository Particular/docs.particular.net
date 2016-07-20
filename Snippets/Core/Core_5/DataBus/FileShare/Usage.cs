namespace Core5.DataBus.FileShare
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration, string databusPath)
        {
            #region FileShareDataBus

            var dataBus = busConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(databusPath);

            #endregion
        }

    }




}