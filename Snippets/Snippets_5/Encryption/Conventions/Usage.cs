namespace Snippets5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region DefiningEncryptedPropertiesAs
            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
