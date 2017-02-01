namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService
            NServiceBus.Encryption.MessageProperty.ConfigureRijndaelEncryptionService.RegisterEncryptionService(endpointConfiguration, () => new EncryptionService());

            #endregion
        }

    }
}