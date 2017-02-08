namespace Core6.UpgradeGuides.Split.EncryptionService
{
    using Core6.Encryption.EncryptionService;
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SplitEncryptionFromIEncryptionService
            endpointConfiguration.EnableMessagePropertyEncryption(new EncryptionService());
            #endregion
        }

    }
}