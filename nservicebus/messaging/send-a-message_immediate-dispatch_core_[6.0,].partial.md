The preferred approach is to use the the explicit immediate dispatch API:

snippet: RequestImmediateDispatch

WARNING: By specifying immediate dispatch, outgoing messages will not be [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.

Suppressing the ambient transaction to have the outgoing message sent immediately is also possible:

snippet: RequestImmediateDispatchUsingScope

WARNING: Suppressing transaction scopes only works for MSMQ and SQL transports in DTC mode. Other transports or disabled DTC may result in unexpected behavior.