namespace Snippets5.PubSub.WithConvention
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region DefiningEventsAs

            BusConfiguration busConfiguration = new BusConfiguration();
            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningEventsAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("Domain") &&
                t.Name.EndsWith("Event"));

            #endregion
        }
    }
}