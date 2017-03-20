This can be done by using the immediate dispatch API:

snippet: RequestImmediateDispatch

WARNING: By specifying immediate dispatch, outgoing messages will not be [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.