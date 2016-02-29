namespace Snippets6.DataBus.Conventions
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = null;
            #region DefineMessageWithLargePayloadUsingConvention

            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
