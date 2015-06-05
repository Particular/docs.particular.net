namespace Snippets5.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region EncryptionFromIEncryptionService

            BusConfiguration busConfiguration = new BusConfiguration();
            //where MyCustomEncryptionService implements IEncryptionService 
            busConfiguration.RegisterEncryptionService(b => new EncryptionService());

            #endregion
        }

    }
}
