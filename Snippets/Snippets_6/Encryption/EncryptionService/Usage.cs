namespace Snippets5.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region EncryptionFromIEncryptionService

            EndpointConfiguration configuration = new EndpointConfiguration();
            //where EncryptionService implements IEncryptionService 
            configuration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
