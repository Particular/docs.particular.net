namespace Core6.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEventsAs
            var conventionsBuilder = endpointConfiguration.Conventions();
            conventionsBuilder.DefiningEventsAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("Domain") &&
                t.Name.EndsWith("Event"));

            #endregion
        }
    }
}