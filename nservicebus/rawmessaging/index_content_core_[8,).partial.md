Since version 8 of NServiceBus the transport infrastructure can be used directly without the need to spin up full NServiceBus endpoint. This is especially useful when dealing with integrations with 3rd party systems and building message gateways or bridges.


## Configuration

Configuration of the messaging infrastructure is done via the `Initialize` method:

snippet: Configuration


## Sending

The following code sends a message to another endpoint using an `IMessageDispatcher` that is part of the initialized infrastructure:

snippet: Sending


## Receiving

The following code starts the configured receiver (identified by `"1"`). Each infrastructure object can contain multiple receivers. Each receiver can be started separately. Once stopped, receivers cannot be restarted. If pause-like functionality is needed create a brand new infrastructure object each time.

snippet: Receiving


## Shutting down

snippet: Shutdown

Before shutting down the infrastructure, make sure to stop all the receivers.