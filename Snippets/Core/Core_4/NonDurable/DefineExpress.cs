namespace Core4.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(Configure configure)
        {
            #region ExpressMessageConvention

            configure.DefiningExpressMessagesAs(
                type =>
                {
                    return type.Name.EndsWith("Express");
                });

            #endregion
        }

    }
}