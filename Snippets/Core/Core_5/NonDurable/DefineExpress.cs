namespace Core5.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(BusConfiguration busConfiguration)
        {
            #region ExpressMessageConvention

            var conventions = busConfiguration.Conventions();
            conventions.DefiningExpressMessagesAs(type =>
            {
                return type.Name.EndsWith("Express");
            });

            #endregion
        }

    }
}