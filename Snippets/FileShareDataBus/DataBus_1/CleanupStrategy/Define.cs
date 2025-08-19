using NServiceBus;

namespace CleanupStrategy
{
    using NServiceBus.ClaimCheck;

    class Define
    {
        Define(EndpointConfiguration endpointConfiguration)
        {
            #region DefineFileLocationForDatabusFiles

            var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
            claimCheck.BasePath(@"\\share\databus_attachments\");

            #endregion
        }
    }
}