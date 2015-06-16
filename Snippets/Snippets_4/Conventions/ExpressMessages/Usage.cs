namespace Snippets4.Conventions.ExpressMessages
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ExpressMEssageConvention

            Configure configure = Configure.With()
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}