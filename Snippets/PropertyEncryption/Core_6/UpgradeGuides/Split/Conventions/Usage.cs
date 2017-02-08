namespace Core6.UpgradeGuides.Split.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SplitDefiningEncryptedPropertiesAs

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