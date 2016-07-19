namespace Core5.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(BusConfiguration busConfiguration)
        {
            #region ExpressMessageConvention

            var conventions = busConfiguration.Conventions();
            conventions.DefiningExpressMessagesAs(t =>
            {
                return t.Name.EndsWith("Express");
            });

            #endregion
        }

    }
}