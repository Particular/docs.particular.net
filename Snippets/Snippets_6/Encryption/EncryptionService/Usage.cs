namespace Snippets5.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region EncryptionFromIEncryptionService

            BusConfiguration busConfiguration = new BusConfiguration();
            //where EncryptionService implements IEncryptionService 
            busConfiguration.RegisterEncryptionService(() => new EncryptionService());

            #endregion
        }

    }
}
