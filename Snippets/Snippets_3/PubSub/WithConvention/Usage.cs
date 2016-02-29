namespace Snippets3.PubSub.WithConvention
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region DefiningEventsAs

            Configure configure = Configure.With();
            configure.DefiningEventsAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("Domain") &&
                t.Name.EndsWith("Event"));

            #endregion
        }

    }
}