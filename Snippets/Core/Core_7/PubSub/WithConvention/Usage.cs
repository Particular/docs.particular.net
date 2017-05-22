namespace Core6.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEventsAs

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(
                type =>
                {
                    return type.Namespace != null &&
                           type.Namespace.StartsWith("Domain") &&
                           type.Name.EndsWith("Event");
                });

            #endregion
        }
    }
}