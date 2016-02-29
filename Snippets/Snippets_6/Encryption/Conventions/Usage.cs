namespace Snippets6.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region DefiningEncryptedPropertiesAs
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
