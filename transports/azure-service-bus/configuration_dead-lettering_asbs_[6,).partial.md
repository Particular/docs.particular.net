### Dead lettering

Azure Service Bus provides a native dead-letter queue (DLQ) for each queue and subscription. NServiceBus can integrate with this mechanism, allowing failed messages to be dead-lettered natively and forwarded to the central NServiceBus error queue.

For background information on Azure Service Bus dead-letter queues, see [Overview of Service Bus dead-letter queues](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dead-letter-queues).

#### Forward dead-lettered messages to the error queue

When queues are created by the transport, native dead-lettered messages can be auto-forwarded to the configured error queue:

```csharp
var transport = new AzureServiceBusTransport("<connection string>", TopicTopology.Default)
{
    AutoForwardDeadLetteredMessagesToErrorQueue = true
};
```

This setting is opt-in and affects only entities created by the transport. See the [`asb-transport` provisioning commands](/transports/azure-service-bus/operational-scripting.md) for more details on scripting options.

#### Route all failed messages to the native DLQ

To route failed messages to the native Azure Service Bus dead-letter queue instead of the NServiceBus error queue, enable:

```csharp
var recoverability = endpointConfiguration.Recoverability();
recoverability.MoveErrorsToAzureServiceBusDeadLetterQueue();
```

#### Request dead lettering from recoverability

Use a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md) to explicitly request dead lettering for selected failures.

Dead-letter with standard NServiceBus fault metadata:

```csharp
endpointConfiguration.Recoverability()
    .CustomPolicy((config, errorContext) =>
    {
        if (errorContext.Exception is PoisonMessageException)
        {
            return RecoverabilityAction.DeadLetter();
        }

        return DefaultRecoverabilityPolicy.Invoke(config, errorContext);
    });
```

Dead-letter with custom reason, description, and modified application properties:

> [!NOTE]
> Using this option does note automatically add the NServiceBus faults metadata to the application properties.

```csharp
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
```

#### Fault header mapping

When processing dead-lettered messages, the transport maps native dead-letter properties to [error forwarding headers](/nservicebus/messaging/headers.md#error-forwarding-headers) when those headers are not already present:

- `DeadLetterSource` -> `NServiceBus.FailedQ`
- `DeadLetterReason` -> `NServiceBus.ExceptionInfo.Message`
- `DeadLetterErrorDescription` -> `NServiceBus.ExceptionInfo.StackTrace`

This mapping helps tools such as ServiceControl and ServicePulse present failure information consistently.

#### Monitoring and operations

[ServicePulse failed message monitoring](/servicepulse/intro-failed-messages.md) tracks messages in the NServiceBus error queue. If endpoint failures are kept in native Azure Service Bus dead-letter queues without forwarding, those failures require Azure-native operational tooling.

Enable dlq forwarding described above when you want native dead lettering and centralized failed-message handling together.

#### Caveats

- `TransportTransactionMode.None` uses receive-and-delete semantics, so dead-lettering actions cannot be performed in that mode. See [transport transactions](/transports/transactions.md#transaction-modes-unreliable-transactions-disabled).
- The transport truncates dead-letter reason and description to 1024 characters to reduce oversized message risk. Review Azure limits in [Service Bus quotas](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas).
