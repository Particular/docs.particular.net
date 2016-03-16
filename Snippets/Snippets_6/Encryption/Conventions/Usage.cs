namespace Snippets6.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region DefiningEncryptedPropertiesAs
            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
