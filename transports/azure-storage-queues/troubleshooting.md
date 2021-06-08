---
title: Azure Storage Queues Troubleshooting
component: ASQ
reviewed: 2021-06-08
---

## Message size too large

When you experience the following exception the cause might be that the message that is sent is too large.

```txt
Microsoft.WindowsAzure.Storage.StorageException: Element 0 in the batch returned an unexpected response code.
```

Reduce the message body by using the [data bus](https://docs.particular.net/nservicebus/messaging/databus/), use [message streams](https://docs.particular.net/samples/pipeline/stream-properties/) to offload data external to the message, apply [message compression](https://www.nuget.org/packages/NServiceBus.Compression/) use a [compact binary serializer](https://docs.particular.net/nservicebus/community/#serializers) or simply store less data in the message.
