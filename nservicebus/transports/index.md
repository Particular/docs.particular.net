---
title: Transports
summary: Transports.
tags: []
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
---

NServiceBus is built on top of exiting queuing technologies. In NServicebus the choice of queuing technology is referred to as a "Transport".

## Avaliable transports

### [MSMQ](/nservicebus/msmq)

MSMQ is the default transport used by NServiceBus.

### [SqlServer](/nservicebus/sqlserver)

Uses Sql Server tables as a queuing mechanism.

### [Azure](/nservicebus/azure)

Uses either Azure Servicebus or Azure Storage Queues as a transport.

### Community run transports

There are several community run transports that can be seen on the full list of [Extensions](/platform/extensions.md#transports).

### WebSphereMQ

WebSphereMQ Transport for NServiceBus is not supported by Particular Software at this time. The code is available as-is, for legacy, community use and reference. https://github.com/ParticularLabs/NServiceBus.WebSphereMQ.

[Contact Particular Software support](http://particular.net/ContactUs) for licensing and support details.
