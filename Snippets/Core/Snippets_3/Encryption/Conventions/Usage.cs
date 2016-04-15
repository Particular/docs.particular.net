namespace Core3.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region DefiningEncryptedPropertiesAs

            configure.DefiningEncryptedPropertiesAs(x => x.Name.EndsWith("EncryptedProperty"));

            #endregion
        }
    }
}