The MSMQ native TTBR implementation can be **disabled** for messages sent as part of a transaction. In this case, messages rely on [non-native TTBR handling](#non-native) to ensure they are discarded **at receive time** if the Time-To-Be-Received has expired. 

Messages sent **outside of a transaction** will still use MSMQ's built-in native TTBR functionality.

snippet: disable-native-ttbr

> [!WARNING]
> Messages sent **without** the native MSMQ `TimeToBeReceived` property cannot be automatically removed from the queue by MSMQ. They will remain in the queue until they are read by a receiving endpoint.
>
> If such a message is received by an endpoint running **NServiceBus.Transport.Msmq version 1.0.x or earlier**, the TTBR header will be ignored and the message may be processed even if it has already expired.