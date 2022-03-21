namespace Core7
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, string databusPath)
        {
            #region FileShareDataBus

            var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(databusPath);

            #endregion
        }
    }




}