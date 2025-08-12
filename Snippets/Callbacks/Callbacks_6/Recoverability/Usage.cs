namespace Callbacks.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        void Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, ILog log)
        {
            #region Callbacks-Recoverability

            endpointConfiguration.Recoverability().CustomPolicy((config, context) =>
            {
                if (context.Exception is InvalidOperationException invalidOperationException && 
                    invalidOperationException.Message.StartsWith("No handlers could be found", StringComparison.OrdinalIgnoreCase))
                {
                    return RecoverabilityAction.Discard("Callback no longer active");
                }
                return DefaultRecoverabilityPolicy.Invoke(config, context);
            });

            #endregion
        }
    }
}