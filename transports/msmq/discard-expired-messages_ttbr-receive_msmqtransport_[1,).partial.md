NServiceBus.Transport.Msmq version 1.0.x and earlier does **not** evaluate the `NServiceBus.TimeToBeReceived` header on incoming messages.

Starting with version 1.1, the transport will discard a message **without processing it** if **all** of the following conditions are met:

- The message contains an `NServiceBus.TimeSent` header.
- The message contains an `NServiceBus.TimeToBeReceived` header.
- The calculated cut-off time (Time Sent + Time To Be Received) has already passed.

> [!WARNING]
> The `NServiceBus.TimeSent` header reflects the **sender’s** system clock, while the cut-off time is evaluated using the **receiver’s** system clock. If the clocks are not properly synchronized, this can lead to messages being discarded prematurely.
>
> Avoid setting TTBR values in seconds or very short durations. If the allowed clock drift is up to 15 seconds and the TTBR is 30 seconds, the TTBR should instead be set to at least 45 seconds to prevent unintended expiration.
>
> Learn more about [clock synchronization issues](/nservicebus/messaging/discard-old-messages.md#clock-synchronization-issues).

If necessary, the transport can be configured to **ignore** the `NServiceBus.TimeToBeReceived` header on incoming messages:

snippet: ignore-incoming-ttbr-headers