namespace Core4.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {

        Usage(Configure configure)
        {
            #region DefiningEncryptedPropertiesAs

            configure.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));

            #endregion
        }
    }
}