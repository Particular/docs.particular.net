﻿namespace Core6.Errors.ErrorQueue
{
    using NServiceBus;

    public class ErrorHeadersCustomization
    {
        public void ConfigureErrorHeadersCustomizations(EndpointConfiguration configuration)
        {
            #region ErrorHeadersCustomizations 6

            var recoverability = configuration.Recoverability();
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