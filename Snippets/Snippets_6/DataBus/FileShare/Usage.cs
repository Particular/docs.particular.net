namespace Snippets6.DataBus.FileShare
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            string databusPath = null;

            #region FileShareDataBus

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}