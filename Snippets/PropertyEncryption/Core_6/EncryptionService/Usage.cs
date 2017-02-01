namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService
            endpointConfiguration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
