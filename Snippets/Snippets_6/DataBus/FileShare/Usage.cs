namespace Snippets6.DataBus.FileShare
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            string databusPath = null;

            #region FileShareDataBus

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}