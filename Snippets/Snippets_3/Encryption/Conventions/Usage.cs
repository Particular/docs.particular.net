
namespace Snippets3.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region DefiningEncryptedPropertiesAs

            Configure configure = Configure.With();
            configure.DefiningEncryptedPropertiesAs(x => x.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
