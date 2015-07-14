namespace Snippets5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region DefiningEncryptedPropertiesAs
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Conventions()
                .DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
