
namespace Snippets3.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            Configure configure = Configure.With();
            #region DefiningEncryptedPropertiesAs

            configure.DefiningEncryptedPropertiesAs(x => x.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
