namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService
            // where EncryptionService implements IEncryptionService 
            endpointConfiguration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
