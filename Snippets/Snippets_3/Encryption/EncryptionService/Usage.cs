namespace Snippets3.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption;

    public class Usage
    {
        public Usage()
        {
            #region EncryptionFromIEncryptionService

            //where MyCustomEncryptionService implements IEncryptionService 
            Configure.With()
                .Configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }
    }
}