namespace Snippets4.NonDurable.ExpressMessages
{
    using NServiceBus;

    class DefineExpress
    {
        DefineExpress(Configure configure)
        {
            #region ExpressMessageConvention

            configure.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}