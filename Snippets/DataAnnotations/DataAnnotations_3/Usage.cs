using NServiceBus;

public class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region DataAnnotations

        endpointConfiguration.UseDataAnnotationsValidation();

        #endregion

        #region DataAnnotations_disableincoming

        endpointConfiguration.UseDataAnnotationsValidation(
            incoming: false);

        #endregion

        #region DataAnnotations_disableoutgoing

        endpointConfiguration.UseDataAnnotationsValidation(
            outgoing: false);

        #endregion
    }
}