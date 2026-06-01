---
title: Azure Storage Queues Troubleshooting
summary: Tips for troubleshooting the Azure Storage Queues persister
component: ASQ
reviewed: 2026-02-25
---

## Message size too large

When experiencing the following exception, the cause might be that the message that is sent is too large.

```txt
Microsoft.WindowsAzure.Storage.StorageException: Element 0 in the batch returned an unexpected response code.
```

Reduce the message body with one or more of the following techniques:

- use the [`DataBus` feature](/nservicebus/messaging/claimcheck/)
- apply [message compression](https://www.nuget.org/packages/NServiceBus.Compression/)
- use a custom binary serializer
- include less data in the message.
