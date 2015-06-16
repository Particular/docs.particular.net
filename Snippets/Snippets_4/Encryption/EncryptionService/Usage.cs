
namespace Snippets4.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption;

    class Usage
    {

        public void FromCustomIEncryptionService()
        {
            #region EncryptionFromIEncryptionService
            //where EncryptionService implements IEncryptionService 
            Configure.With()
                .Configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());
            #endregion
        }

    }
}
