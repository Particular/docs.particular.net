namespace Snippets6.DataBus.FileShare
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            string databusPath = null;

            #region FileShareDataBus

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseDataBus<FileShareDataBus>()
                .BasePath(databusPath);

            #endregion
        }

    }




}