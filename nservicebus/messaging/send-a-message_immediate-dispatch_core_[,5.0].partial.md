This can be done by suppressing the ambient transaction:

snippet: RequestImmediateDispatchUsingScope

WARNING: Suppressing transaction scopes only works for MSMQ and SQL transports in DTC mode. Other transports or disabled DTC may result in unexpected behavior.