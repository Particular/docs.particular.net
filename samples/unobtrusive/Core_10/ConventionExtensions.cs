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
#pragma warning disable CS0618 // Type or member is obsolete
        conventions.DefiningDataBusPropertiesAs(
            property => property.Name.EndsWith("DataBus"));
#pragma warning restore CS0618 // Type or member is obsolete

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