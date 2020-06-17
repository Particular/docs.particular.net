The MSMQ native TTBR implementation can be disabled for messages sent as part of a transaction. These messages rely on the [non-native](#non-native) TTBR handling to ensure they are discarded when they are read by an endpoint, if the time to be received has expired. Messages sent outside of a transaction will still use the native TTBR capabilities built in to the transport.

```csharp
var transport = endpointConfiguration.UseTransport<MsmqTransport>();
transport.DisableNativeTimeToBeReceivedInTransactions();
```

WARN: Messages sent without the native MSMQ TTBR property set cannot automatically be cleaned up by MSMQ. They will remain in a queue until they are read. If they are read by an endpoint running on MSMQ transport 1.0.x or below, messages can be processed even if the TTBR header has expired.