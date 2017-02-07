namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService
            endpointConfiguration.EnableMessagePropertyEncryption(new EncryptionService());
            #endregion
        }

    }
}