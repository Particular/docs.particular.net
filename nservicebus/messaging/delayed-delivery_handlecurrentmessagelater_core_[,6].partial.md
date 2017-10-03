## Delaying messages using HandleCurrentMessageLater

The `HandleCurrentMessageLater()` method was primarily used to defer messages when using Versions 2.x and below, before the Defer functionality was introduced in Versions 3 and above. While this API is still supported in Versions 6 and below, there are significant caveats in using this API.

 * Calling this method would create a copy of the message that has the same identifier, header, and body. This message would then be put at the end of the queue. The endpoint will eventually pick up this message once all the other messages in its queue have been processed. To make this work, the message pipeline will not abort which means any business transaction that's part of calling this method will also get committed.
 * If the endpoint's queue is empty, or the condition to put the message back into the queue is incorrect, the message goes back into the queue immediately causing the endpoint to process the same message without any delay. This behavior can cause an endless loop which will manifest itself as a high system resource utilization by the endpoint.


WARNING: `HandleCurrentMessageLater()` cannot be used in conjunction with the Outbox.

WARNING: This method will be deprecated in Version 7.0. It is recommended to use either [Delayed Retries](/nservicebus/recoverability/#delayed-retries) or one of the deferring mechanisms below, depending on the scenario.