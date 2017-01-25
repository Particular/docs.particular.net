namespace Core6.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEncryptedPropertiesAs

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(property =>
            {
                return property.Name.EndsWith("EncryptedProperty");
            });

            #endregion
        }
    }
}