namespace Snippets4.DataBus.Azure
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureDataBus

            Configure configure = Configure.With();
            configure.AzureDataBus();

            #endregion
        }
    }
}
