using NServiceBus;

public class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region DataAnnotations

        endpointConfiguration.UseDataAnnotationsValidation();

        #endregion

        #region DataAnnotations_outgoing

        endpointConfiguration.UseDataAnnotationsValidation(
            validateOutgoingMessages:true);

        #endregion
    }
}