namespace Snippets5.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefiningEventsAs

            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningEventsAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("Domain") &&
                t.Name.EndsWith("Event"));

            #endregion
        }
    }
}