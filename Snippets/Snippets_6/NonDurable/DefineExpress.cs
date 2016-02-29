namespace Snippets6.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress()
        {
            #region ExpressMessageConvention

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            ConventionsBuilder builder = endpointConfiguration.Conventions();
            builder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}