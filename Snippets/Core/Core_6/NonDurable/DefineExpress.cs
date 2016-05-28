namespace Core6.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(EndpointConfiguration endpointConfiguration)
        {
            #region ExpressMessageConvention
            var conventionsBuilder = endpointConfiguration.Conventions();
            conventionsBuilder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}