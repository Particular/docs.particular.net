
namespace Snippets3.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region DefiningEncryptedPropertiesAs
            Configure.With()
                .DefiningEncryptedPropertiesAs(x => x.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
