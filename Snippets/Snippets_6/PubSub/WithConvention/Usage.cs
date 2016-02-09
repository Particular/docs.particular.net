namespace Snippets6.PubSub.WithConvention
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region DefiningEventsAs

            EndpointConfiguration configuration = new EndpointConfiguration();
            ConventionsBuilder conventions = configuration.Conventions();
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Domain") && t.Name.EndsWith("Event"));
            #endregion
        }
    }
}