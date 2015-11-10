namespace Snippets3.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress()
        {
            #region ExpressMessageConvention

            Configure configure = Configure.With();
            configure.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}