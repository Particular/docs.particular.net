namespace Snippets6.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress(EndpointConfiguration endpointConfiguration)
        {
            #region ExpressMessageConvention
            ConventionsBuilder builder = endpointConfiguration.Conventions();
            builder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}