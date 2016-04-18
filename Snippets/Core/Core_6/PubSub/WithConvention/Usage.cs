namespace Core6.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEventsAs
            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("Domain") &&
                t.Name.EndsWith("Event"));

            #endregion
        }
    }
}