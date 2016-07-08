
## Cancellation

This API was added in the externalized Callbacks feature.

The asynchronous callback can be canceled by registering a `CancellationToken` provided by a `CancellationTokenSource`. The token needs to be registered on the `SendOptions` as shown below.

snippet:CancelCallback