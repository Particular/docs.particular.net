## Exception Handling

Exceptions thrown from a behaviors `Invoke` method bubble up the chain. If the exception is not handled by a behavior, the message is considered as faulted which results in putting the message back in the queue (and rolling back the transaction) or moving it to the error queue (depending on the endpoint configuration).


### MessageDeserializationException (Version 5.1 and above)

If a message fails to deserialize a `MessageDeserializationException` will be thrown by the  `DeserializeLogicalMessagesBehavior`. In this case, the message is directly moved to the error queue to avoid blocking the system by poison messages.
