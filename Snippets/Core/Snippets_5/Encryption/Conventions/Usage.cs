namespace Snippets5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefiningEncryptedPropertiesAs
            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
