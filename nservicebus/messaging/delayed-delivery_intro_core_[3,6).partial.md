

Calling `bus.Defer(...)` the handling of the message is deferred by certain amount of time. After that time the message is passed to be handled by the same endpoint. Semantically `bus.Defer(...)` is equivalent to calling `bus.SendLocal(...)` after time condition is met. See also [sending local](/nservicebus/messaging/send-a-message.md#sending-to-self).

Using this mechanism delayed delivery can be achieved by calling `bus.Defer(...)` which can be seen in [Delayed Delivery Sample](/samples/delayed-delivery).
