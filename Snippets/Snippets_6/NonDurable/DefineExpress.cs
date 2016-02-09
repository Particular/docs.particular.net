namespace Snippets6.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress()
        {
            #region ExpressMessageConvention

            EndpointConfiguration configuration = new EndpointConfiguration();
            ConventionsBuilder builder = configuration.Conventions();
            builder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}