namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using NServiceBus;
    using NServiceBus.Transport;

    class CustomExceptionPolicy
    {
        CustomExceptionPolicy(EndpointConfiguration endpointConfiguration)
        {
            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Delayed(delayed => delayed.NumberOfRetries(3));
            recoverabilitySettings.CustomPolicy(MyCustomRetryPolicy);
        }

        #region CustomExceptionPolicyHandler
        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            if (context.Exception is MyBusinessException)
            {
                return RecoverabilityAction.MoveToError();
            }

            return DefaultRecoverabilityPolicy.Invoke(config, context);
        }
        #endregion
    }
}