namespace Core8.Recoverability.ErrorHandling
{
    using NServiceBus;

    public class ErrorHeadersCustomization
    {
        public void ConfigureErrorHeadersCustomizations(EndpointConfiguration endpointConfiguration)
        {
            #region ErrorHeadersCustomizations

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Failed(
                failed =>
                {
                    failed.HeaderCustomization(headers =>
                    {
                        if (headers.ContainsKey("NServiceBus.ExceptionInfo.Message"))
                        {
                            headers["NServiceBus.ExceptionInfo.Message"] = "message override";
                        }
                    });
                });

            #endregion
        }
    }
}