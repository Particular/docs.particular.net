`NServiceBus.Raw` allows sending and receiving of raw messages using the [NServiceBus transport infrastructure](/transports/). It is flexible in terms of message manipulation, and is a good fit for integration with third-party systems, as well as building gateways and bridges.

## Configuration

Configuration of raw endpoints is similar to the standard NServiceBus [endpoint configuration](/nservicebus/endpoints/specify-endpoint-name.md):

snippet: Configuration

## Sending

The following code sends a message to another endpoint:

snippet: Sending

## Receiving

The following code implements the callback invoked when a message arrives at a raw endpoint:

snippet: Receiving

Notice that the method gets a `dispatcher` object which can be used to send messages. The `TransportTransaction` object can be passed from the receiving context to the dispatcher, in order to ensure transactions span both send and receive operations. It's important to ensure that the underlying transport infrastructure supports the [`SendsAtomicWithReceive` transaction mode](/transports/transactions.md#transaction-modes-transport-transaction-sends-atomic-with-receive) when using this option.
