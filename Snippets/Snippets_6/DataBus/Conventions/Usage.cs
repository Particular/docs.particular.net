namespace Snippets6.DataBus.Conventions
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration configuration = null;
            #region DefineMessageWithLargePayloadUsingConvention

            ConventionsBuilder conventions = configuration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
