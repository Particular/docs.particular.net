namespace Core6.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEncryptedPropertiesAs
            var conventionsBuilder = endpointConfiguration.Conventions();
            conventionsBuilder.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
