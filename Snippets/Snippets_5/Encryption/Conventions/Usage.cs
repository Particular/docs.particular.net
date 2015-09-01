namespace Snippets5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region DefiningEncryptedPropertiesAs
            BusConfiguration busConfiguration = new BusConfiguration();
            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
