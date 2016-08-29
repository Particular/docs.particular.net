---
title: Azure Service Bus Common Exceptions
reviewed: 2016-08-29
tags:
- Azure
- Cloud
- Error Handling
---

## Common Exceptions

This document lists common exceptions logged by NServiceBus, their potential cause and resolution.

| Exception   | Message  | Potential cause and resolution  |
|---|---|---|
| Microsoft.ServiceBus.Messaging.MessageLockLostException | The lock supplied is invalid. Either the lock expired, or the message has already been removed from the queue. | The endpoint is too slow in processing messages that it pulls from the queue. This can usually be resolved by reducing the batch size or extending lock duration, but would ideally be responded to by increasing the performance of the handler implementation. |
| System.ServiceModel.FaultException | A timeout has occurred during the operation. | This exception points at a network problem between the endpoint's host machine and the broker service, including the local network, internet infrastructure as well as the Azure Datacenter edge network. |
| System.TimeoutException | The timeout elapsed upon attempting to obtain a token while accessing 'https://{yournamespace}.accesscontrol.windows.net/WRAPv0.9/' | The SDK was unable to communicate with Azure Access Control (ACS) Service in order to obtain an access token for authentication on the ASB service. It is advised to use Shared Access Signature (SAS) authentication instead. |
