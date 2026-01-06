---
title: Configure error handling
summary: Configure handling of failed messages
component: Core
reviewed: 2026-01-05
redirects:
- nservicebus/configure-error-queue
related:
 - samples/faulttolerance
 - samples/custom-recoverability
---

## Configure the error queue address

When an endpoint fails to process a message successfully, NServiceBus [automatically retries](/nservicebus/recoverability/configure-immediate-retries.md) the message the configured number of times. If the message can not be processed successfully even after the retried attempts, NServiceBus forwards the message to a designated error queue.

> [!WARNING]
> When running with [transport transactions disabled](/transports/transactions.md#transaction-modes-unreliable-transactions-disabled), NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

> [!WARNING]
> When running with [transport transactions disabled](/transports/transactions.md#transaction-modes-unreliable-transactions-disabled). Both [immediate retries](/nservicebus/recoverability/#immediate-retries) and [delayed retries](/nservicebus/recoverability/#delayed-retries) will be disabled automatically when transactions are turned off.

The default error queue name is `error` but some transports require it to be explicitly configured.

### Using code

snippet: ErrorWithCode

include: configurationWarning

## Error message header customizations

Before a message is moved to the error queue, it is possible to inspect and modify the [error forwarding headers](/nservicebus/messaging/headers.md#error-forwarding-headers).

> [!WARNING]
> Before Version 8, modifying existing headers on the failed message was possible. Starting in Version 8, use a [recoverability pipeline behavior](/nservicebus/recoverability/pipeline.md) to get full access to all headers. See the [upgrade guide](/nservicebus/upgrades/7to8/#header-manipulation-for-failed-messages) for more information.

The following snippet shows how to configure header customizations and perform header value modification.

snippet: ErrorHeadersCustomizations


## Error queue monitoring

Administrators should monitor the error queue in order to detect when problems occur. The message in the error queue contains relevant information such as the endpoint that initially processed the message and exception details. This allows an administrator to investigate the problem.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse/) provides access to full exception details, including the stack-trace. ServicePulse offers advanced debugging capabilities, providing additional information like exception details as well as visualizing the flow of messages. They also provide `retry` functionality, which sends a failed message from the error queue back to the originating endpoint for re-processing. For more details on how to retry a message using ServicePulse, see [Failed Message Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). To retry a message using ServicePulse, see [Retrying Failed Messages in ServicePulse](/servicepulse/intro-failed-message-retries.md).

If ServicePulse is not available in the environment, the message retry functionality can be performed using the native management tools appropriate for the selected transport:

 * [MSMQ Scripting](/transports/msmq/operations-scripting.md)
 * [RabbitMQ Scripting](/transports/rabbitmq/operations-scripting.md)
 * [SQL Server Scripting](/transports/sql/operations-scripting.md)
