using NServiceBus;

class Usage
{
    Usage(NServiceBus.EndpointConfiguration endpointConfiguration, string databusPath)
    {
        #region FileShareDataBus

        var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
        claimCheck.BasePath(databusPath);

        #endregion
    }
}
