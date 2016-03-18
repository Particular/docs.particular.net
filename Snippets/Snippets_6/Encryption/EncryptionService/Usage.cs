namespace Snippets6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService
            //where EncryptionService implements IEncryptionService 
            endpointConfiguration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
