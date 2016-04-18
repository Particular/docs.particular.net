namespace Core4.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption;

    class Usage
    {

        Usage(Configure configure)
        {
            #region EncryptionFromIEncryptionService

            //where EncryptionService implements IEncryptionService 
            configure.Configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }
    }
}