namespace Core5
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