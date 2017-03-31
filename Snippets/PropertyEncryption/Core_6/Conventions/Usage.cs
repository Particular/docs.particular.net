#pragma warning disable 618
namespace Core6.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEncryptedPropertiesAs

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(
                definesEncryptedProperty: propertyInfo =>
                {
                    return propertyInfo.Name.EndsWith("EncryptedProperty");
                });

            #endregion
        }
    }
}