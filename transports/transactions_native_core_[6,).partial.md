### Transport transaction - Receive Only

In this mode the receive operation is wrapped in a transport's native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See also [recoverability](/nservicebus/recoverability/) for more details on retries.

NOTE: [Sends and Publishes are batched](/nservicebus/messaging/batched-dispatch.md) and only transmitted until all handlers are successfully invoked. 
Messages that are required to be sent immediately should use the [immediate dispatch option](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) which bypasses batching.

Use the following code to use this mode:

snippet: TransportTransactionReceiveOnly


#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded updating its data but the other didn't
 * partial sends - where some of the messages have been sent but others not

When using this mode all handlers must be [idempotent](/nservicebus/concept-overview.md#idempotence), i.e. the result needs to be consistent from a business perspective even when the message is processed more than once.

See the `Outbox` section below for details on how NServiceBus can handle idempotency at the infrastructure level.


### Transport transaction - Sends atomic with Receive

Some transports support enlisting outgoing operations in the current receive transaction. This prevents messages being sent to downstream endpoints during retries.

Use the following code to use this mode:

snippet: TransportTransactionAtomicSendsWithReceive


#### Consistency guarantees

This mode has the same consistency guarantees as the *Receive Only* mode, but additionally it prevents occurrence of *ghost messages* since all outgoing operations are atomic with the ongoing receive operation.
