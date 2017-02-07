namespace Core3.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region DefiningEncryptedPropertiesAs

            configure.DefiningEncryptedPropertiesAs(
                definesEncryptedProperty: propertyInfo =>
                {
                    return propertyInfo.Name.EndsWith("EncryptedProperty");
                });

            #endregion
        }
    }
}