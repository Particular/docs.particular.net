---
title: Azure Storage Queues Sanitization
component: ASQ
versions: '[8,)'
related:
- transports/azure-storage-queues/configuration
- samples/azure/storage-queues
reviewed: 2020-11-09
---

Starting with NServiceBus.Azure.Transports.WindowsAzureStorageQueues version 8.0, the transport no longer sanitizes queues by default.


## Azure Storage Queues naming rules

 1. A queue name must start with a letter or number, and can only contain letters, numbers, and the dash (`-`) character.
 1. The first and last letters in the queue name must be alphanumeric.
 1. The dash (`-`) character cannot be the first or last character.
 1. Consecutive dash characters are not permitted in the queue name.
 1. All letters in a queue name must be lowercase.
 1. A queue name must be between 3 and 63 characters long.


## Custom sanitization

To sanitize queue names, a sanitization function containing the required logic can be registered.

snippet: azure-storage-queue-sanitization

When an endpoint is started, the sanitizer function will be invoked for each queue the transport creates.


## Backward compatibility with versions 7 and below

To remain backward compatible with endpoints created in version 7 and below of the transport, endpoints created in version 8 and above should be configured to perform sanitization based on version 7 and below rules. The following custom code will ensure queues are sanitized in a backward-compatible manner.

snippet: azure-storage-queue-backwards-compatible-sanitization-with-md5

Sanitization code for MD5 and SHA1:

snippet: azure-storage-queue-backwards-compatible-sanitization


## Future consideration prior to using sanitization

When implementing custom sanitization, consider factors such as readability and discover-ability. Things to consider:

 * Truncated long queue names could conflict.
 * Hashed queue names could lead to difficult names to use during production troubleshooting or debugging.
 * Sanitized queue names stay in the system and cannot be replaced until no longer used.

Possible way to avoid sanitization is to define endpoint name short and meaningful.