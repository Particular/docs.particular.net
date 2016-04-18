namespace Core4.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region DefiningEventsAs
            configure.DefiningEventsAs(t => 
            t.Namespace != null &&
            t.Namespace.StartsWith("Domain") && 
            t.Name.EndsWith("Event"));
            #endregion
        }

    }
}