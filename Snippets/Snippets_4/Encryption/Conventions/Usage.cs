namespace Snippets4.Encryption.Conventions
{
    using NServiceBus;

    public class Usage
    {

        public Usage()
        {
            #region DefiningEncryptedPropertiesAs
            Configure.With()
                .DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }

    }
}
