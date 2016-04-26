namespace Azure_5
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
