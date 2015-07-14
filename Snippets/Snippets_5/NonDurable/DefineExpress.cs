namespace Snippets5.NonDurable.ExpressMessages
{
    using NServiceBus;

    public class DefineExpress
    {
        public DefineExpress()
        {
            #region ExpressMessageConvention

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Conventions()
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

            #endregion
        }

    }
}