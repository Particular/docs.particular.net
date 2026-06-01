using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class DeadLettering
{
    void MoveErrorsToDeadLetterQueue(EndpointConfiguration endpointConfiguration)
    {
        #region dlq-all-errors

        endpointConfiguration.Recoverability()
            .MoveErrorsToAzureServiceBusDeadLetterQueue();

        #endregion
    }

    void ExplicitDlq(EndpointConfiguration endpointConfiguration)
    {
        #region explicit-dlq

        endpointConfiguration.Recoverability()
            .CustomPolicy((config, errorContext) =>
            {
                if (errorContext.Exception is PoisonMessageException)
                {
                    return RecoverabilityAction.DeadLetter();
                }

                return DefaultRecoverabilityPolicy.Invoke(config, errorContext);
            });

        #endregion
    }

    void ExplicitDlqFullControl(EndpointConfiguration endpointConfiguration)
    {
        #region explicit-dlq-full-control

        endpointConfiguration.Recoverability()
            .CustomPolicy((config, errorContext) =>
            {
                if (errorContext.Exception is MyBusinessException ex)
                {
                    return RecoverabilityAction.DeadLetter(
                        deadLetterReason: "Business rule validation failed",
                        deadLetterErrorDescription: ex.Message,
                        propertiesToModify: new Dictionary<string, object>
                        {
                            ["FailureCategory"] = "Validation"
                        });
                }

                return DefaultRecoverabilityPolicy.Invoke(config, errorContext);

            });
        #endregion
    }

    class PoisonMessageException : Exception
    {
    }

    class MyBusinessException : Exception
    {
    }
}