namespace Core5.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EncryptionFromIEncryptionService

            // where EncryptionService implements IEncryptionService 
            busConfiguration.RegisterEncryptionService(b => new EncryptionService());

            #endregion
        }

    }
}
