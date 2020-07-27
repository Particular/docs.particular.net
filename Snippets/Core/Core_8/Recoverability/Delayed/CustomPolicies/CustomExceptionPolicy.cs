namespace Core8.Recoverability.Delayed.CustomPolicies
{
    using NServiceBus;

    class CustomExceptionPolicy
    {
        CustomExceptionPolicy(EndpointConfiguration endpointConfiguration)
        {
            #region CustomExceptionPolicyHandler

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.AddUnrecoverableException<MyBusinessException>();

            #endregion
        }
    }
}