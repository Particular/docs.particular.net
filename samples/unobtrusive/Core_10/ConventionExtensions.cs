using NServiceBus.ClaimCheck;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this EndpointConfiguration endpointConfiguration)
    {
        var conventions = endpointConfiguration.Conventions();

        conventions.DefiningCommandsAs(
            type =>
            type.Namespace != null &&
            type.Namespace.EndsWith("Commands"));

        conventions.DefiningEventsAs(
            type =>
            type.Namespace != null &&
            type.Namespace.EndsWith("Events"));

        conventions.DefiningMessagesAs(
            type => type.Namespace == "Messages");

        conventions.DefiningClaimCheckPropertiesAs(
            property => property.Name.EndsWith("ClaimCheck"));

        conventions.DefiningTimeToBeReceivedAs(
            type =>
            {
                if (type.Name.EndsWith("Expires"))
                {
                    return TimeSpan.FromSeconds(30);
                }
                return TimeSpan.MaxValue;
            });
    }

    #endregion
}