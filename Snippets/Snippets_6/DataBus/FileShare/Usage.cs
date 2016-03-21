namespace Snippets6.DataBus.FileShare
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, string databusPath)
        {
            #region FileShareDataBus
            endpointConfiguration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}