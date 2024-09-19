## Access to the native Azure Storage Queue incoming message

Accessing the native azure storage queue incoming message from behaviors and handlers can be beneficial for customizations and recoverability.  When a message is received, the transport includes the [native `QueueMessage`](https://learn.microsoft.com/en-us/dotnet/api/azure.storage.queues.models.queuemessage?view=azure-dotnet) in the message processing context. You can use the following code to retrieve the message details from a pipeline behavior:

snippet: access-native-incoming-message

The behavior above utilizes the native message’s `NextVisibleOn` property to identify when the message lost its lock due to aggressive prefetching and slow processing. If needed, a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md) can be implemented to bypass retry attempts that would inevitably fail because of the lost lock.
