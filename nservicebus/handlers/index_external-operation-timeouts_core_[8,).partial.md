Create a `CancellationTokenSource` linked to `context.CancellationToken`, so the operation is canceled when either the timeout elapses or the endpoint is [shutting down](/nservicebus/hosting/cooperative-cancellation.md). Pass its token to the external call:

snippet: HandlerExternalOperationTimeout
