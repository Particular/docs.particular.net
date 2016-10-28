namespace Core4.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region DefiningEventsAs

            configure.DefiningEventsAs(
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