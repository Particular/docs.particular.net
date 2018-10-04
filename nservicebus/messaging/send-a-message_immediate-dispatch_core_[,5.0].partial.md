This can be done by suppressing the ambient transaction:

snippet: RequestImmediateDispatchUsingScope

WARNING: Suppressing transaction scopes works only for the MSMQ and SQL transports in DTC mode. Other transports, or disabling DTC, may result in unexpected behavior.