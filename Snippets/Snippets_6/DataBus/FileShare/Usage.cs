namespace Snippets6.DataBus.FileShare
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            string databusPath = null;

            #region FileShareDataBus
            endpointConfiguration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}