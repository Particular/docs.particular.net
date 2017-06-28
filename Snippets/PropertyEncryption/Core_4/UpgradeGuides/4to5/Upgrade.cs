namespace Core4.UpgradeGuides._4to5
{
    using NServiceBus;
    using NServiceBus.Encryption;
    using Encryption.EncryptionService;

    class Upgrade
    {

        public void EncryptionServiceSimple()
        {
            #region 4to5EncryptionServiceSimple

            var configure = Configure.With();
            configure.RijndaelEncryptionService();

            #endregion
        }

        public void FromCustomIEncryptionService()
        {
            #region 4to5EncryptionFromIEncryptionService

            // where EncryptionService implements IEncryptionService
            var configure = Configure.With();
            var components = configure.Configurer;
            components.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }

    }

}