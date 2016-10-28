namespace Core5.PubSub.WithConvention
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefiningEventsAs

            var conventions = busConfiguration.Conventions();
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