---
title: Configure error handling
summary: Error handler configuration when messages are forwarded to the error queue after failing a certain number of times
component: Core
reviewed: 2020-05-05
redirects:
- nservicebus/configure-error-queue
related:
 - samples/faulttolerance
---

## Configure the error queue address

When an endpoint fails to process a message successfully, NServiceBus [automatically retries](/nservicebus/recoverability/configure-immediate-retries.md) the message the configured number of times. If the message can not be processed successfully even after the retried attempts, NServiceBus forwards the message to a designated error queue.

WARNING: When running with [transport transactions disabled](/transports/transactions.md#transactions-unreliable-transactions-disabled), NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

WARNING: When running with [transport transactions disabled](/transports/transactions.md#transactions-unreliable-transactions-disabled). Both [immediate retries](/nservicebus/recoverability/#immediate-retries) and [delayed retries](/nservicebus/recoverability/#delayed-retries) will be disabled automatically when transactions are turned off.

partial: ErrorQueueDefault

partial: ErrorWithCode

include: configurationWarning

partial: errorwithconfig

partial: errorheader


## Error queue monitoring

Administrators should monitor the error queue in order to detect when problems occur. The message in the error queue contains relevant information such as the endpoint that originally processed the message and exception details. With this information, an administrator can investigate the problem and solve it, for example, bringing up a database that went down.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse/) provides access to full exception details including the stack-trace. [ServiceInsight](/serviceinsight/) offers advanced debugging capability providing additional information like exception details as well as visualizing the flow of messages. Both ServiceInsight and ServicePulse provide `retry` functionality that sends a failed message from the error queue back to the originating endpoint for re-processing. For more details on how to retry a message using ServicePulse, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). To retry a message using ServiceInsight, see [Managing Errors and Retries in ServiceInsight](/serviceinsight/managing-errors-and-retries.md).

If either ServicePulse or ServiceInsight are not available in the environment, the `retry` operation can be performed using the native management tools appropriate for the selected transport:

 * [MSMQ Scripting](/transports/msmq/operations-scripting.md)
 * [RabbitMQ Scripting](/transports/rabbitmq/operations-scripting.md)
 * [SQL Server Scripting](/transports/sql/operations-scripting.md)