---
title: Transport seam changes in Version 6
tags:
 - upgrade
 - migration
---


`IDispatchMessages` have been obsoleted and is replaced by `IPushMessages`. The interfaces are equivalent so if implementing a transport, implement the new interface. `PushContext` has been given a new property `PushContext.ReceiveCancellationTokenSource`, revealing the intent of cancellation for receiving the current message. The transport implementation should act accordingly, canceling the receive when the source's token is canceled.

The `ConfigureTransport` class was deprecated. Custom transports are now configured using the `TransportDefinition` class, see [this sample](/samples/custom-transport) for more information.


## Corrupted messages

The core will now pass the error queue address to the transport to make it easier to handle corrupted messages. If a corrupted message is detected the transport is expected to move the message to the specified error queue.
