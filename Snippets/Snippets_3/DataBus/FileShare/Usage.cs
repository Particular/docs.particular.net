namespace Snippets3.DataBus.FileShare
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            string databusPath = null;

            #region FileShareDataBus

            Configure configure = Configure.With()
                .FileShareDataBus(databusPath);

            #endregion
        }

    }

}