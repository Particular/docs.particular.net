namespace Core7.NonDurable
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(EndpointConfiguration endpointConfiguration)
        {
            #region ExpressMessageConvention

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningExpressMessagesAs(
                type =>
                {
                    return type.Name.EndsWith("Express");
                });

            #endregion
        }

    }
}