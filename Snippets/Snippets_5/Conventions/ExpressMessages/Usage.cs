namespace Snippets5.Conventions.ExpressMessages
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ExpressMessageConvention

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}