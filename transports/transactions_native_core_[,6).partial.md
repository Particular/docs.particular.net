### Transport transaction

In this mode the receive operation is wrapped in a transport's native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See also [recoverability](/nservicebus/recoverability/) for more details on retries.

Use the following code to use this mode:

snippet: TransportTransactionReceiveOnly


#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded updating its data but the other didn't
 * partial sends - where some of the messages has been sent but others not

When using this mode all handlers must be [idempotent](/nservicebus/concepts/glossary.md#idempotence). In other words the result needs to be consistent from a business perspective even when the message is processed more than once.

See the `Outbox` section below for details on how NServiceBus can handle idempotency at the infrastructure level.

NOTE: Not all the transport support [batched dispatch](/nservicebus/messaging/batched-dispatch.md) and this means that messages could be sent out without a matching update to business data, depending on the order in which  statements were executed. Such messages are called *ghost messages*. To avoid this situation make sure to perform all bus operations only after modifications to business data. When reviewing the code remember that there can be multiple handlers for a given message and that [handlers are executed in a certain order](/nservicebus/handlers/handler-ordering.md).
