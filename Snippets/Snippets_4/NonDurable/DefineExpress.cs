namespace Snippets4.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress()
        {
            #region ExpressMessageConvention

            Configure configure = Configure.With()
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}