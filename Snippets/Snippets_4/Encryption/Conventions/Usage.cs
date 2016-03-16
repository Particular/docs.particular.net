namespace Snippets4.Encryption.Conventions
{
    using NServiceBus;

    public class Usage
    {

        public Usage()
        {
            Configure configure = Configure.With();
            #region DefiningEncryptedPropertiesAs

            configure.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));

            #endregion
        }
    }
}