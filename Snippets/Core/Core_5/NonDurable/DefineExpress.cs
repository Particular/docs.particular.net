namespace Core5.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(BusConfiguration busConfiguration)
        {
            #region ExpressMessageConvention

            ConventionsBuilder builder = busConfiguration.Conventions();
            builder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}