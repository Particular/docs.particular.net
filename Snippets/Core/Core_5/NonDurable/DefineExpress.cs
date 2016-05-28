namespace Core5.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(BusConfiguration busConfiguration)
        {
            #region ExpressMessageConvention

            var conventionsBuilder = busConfiguration.Conventions();
            conventionsBuilder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}