---
title: Azure Storage Queues Troubleshooting
summary: Tips for troubleshooting the Azure Storage Queues persister
component: ASQ
reviewed: 2021-06-08
---

## Message size too large

When experiencing the following exception the cause might be that the message that is sent is too large.

```txt
Microsoft.WindowsAzure.Storage.StorageException: Element 0 in the batch returned an unexpected response code.
```

Reduce the message body with one or more of the following techniques:

- use the [data bus](/nservicebus/messaging/databus/)
- use [message streams](/samples/pipeline/stream-properties/) to offload data external to the message
- apply [message compression](https://www.nuget.org/packages/NServiceBus.Compression/)
- use a [compact binary serializer](/nservicebus/community/#serializers)
- store less data in the message.
