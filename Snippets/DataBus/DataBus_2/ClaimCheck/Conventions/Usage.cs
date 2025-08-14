namespace ClaimCheck_2.ClaimCheck.Conventions;

using NServiceBus;
using NServiceBus.ClaimCheck;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region DefineMessageWithLargePayloadUsingConvention

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningClaimCheckPropertiesAs(
            property => property.Name.EndsWith("DataBus"));

        #endregion
    }
}