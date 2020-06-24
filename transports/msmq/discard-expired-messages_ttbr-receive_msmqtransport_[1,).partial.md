NServiceBus.Transport.Msmq version 1.0.x or below does not check the `NServiceBus.TimeToBeReceived` header.

NServiceBus.Transport.Msmq version 1.1 and above will consume a message without processing it if all of the following conditions are met:

- The message has an `NServiceBus.TimeSent` header
- The message has an `NServiceBus.TimeToBeReceived` header
- The cut-off time (Time Sent + Time To Be Received) has passed

WARNING: The `NServiceBus.TimeSent` header is based on the clock of the sending machine but the cut-off time is compared to the clock on the receiving machine. There may be issues if the [sending and receiving machines have clock synchronization drift](/nservicebus/messaging/discard-old-messages.md#clock-synchronization-issues). It is not advised to use TTBR values expressed in seconds and if small TTBR durations are used to account for clock drift. If the allowed clock drift is 15 seconds and the TTBR is 30 seconds the TTBR should be set to 45 seconds.

The transport can be configured to ignore the `NServiceBus.TimeToBeReceived` header on incoming messages.

snippet: ignore-incoming-ttbr-headers
