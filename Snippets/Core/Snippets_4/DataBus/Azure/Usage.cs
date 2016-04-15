namespace Snippets4.DataBus.Azure
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region AzureDataBus

            configure.AzureDataBus();

            #endregion
        }
    }
}
