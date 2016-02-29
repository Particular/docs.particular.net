namespace Snippets6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region EncryptionFromIEncryptionService

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            //where EncryptionService implements IEncryptionService 
            endpointConfiguration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
