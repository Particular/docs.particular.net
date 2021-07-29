Starting with NServiceBus version 8, the transport infrastructure can be used directly without the need to spin up a full NServiceBus endpoint. This is especially useful when integrating with third-party systems and when building message gateways or bridges.

## Configuration

Configuration of the messaging infrastructure is done via the `Initialize` method:

snippet: Configuration

## Sending

The following code sends a message to another endpoint using an `IMessageDispatcher` that is part of the initialized infrastructure:

snippet: Sending

## Receiving

The following code starts the configured receiver (identified by ID `"Primary"`). Each infrastructure object can contain multiple receivers. Each receiver can be started separately. Once stopped, receivers cannot be restarted. If pause functionality is needed, create a new infrastructure object each time.

snippet: Receiving

## Shutting down

snippet: Shutdown

Before shutting down the infrastructure, be sure to stop all the receivers.
