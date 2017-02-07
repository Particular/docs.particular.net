namespace Core3.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption;

    class Usage
    {
        Usage(Configure configure)
        {
            #region EncryptionFromIEncryptionService

            // where EncryptionService implements IEncryptionService
            var configurer = configure.Configurer;
            configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }
    }
}