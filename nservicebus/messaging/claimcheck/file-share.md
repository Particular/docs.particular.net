---
title: File Share Data Bus
summary: An implementation of a data bus using file shares
reviewed: 2024-02-16
component: FileShareDataBus
redirects:
 - nservicebus/messaging/databus/file-share
related:
 - samples/databus/file-share-databus
---

The file-share data bus allows large properties to be transferred over a network file share.

This implementation [leverages both serialization and headers](/nservicebus/messaging/headers.md#file-share-data-bus-headers) to provide its functionality.

partial: obsolete

## Usage

snippet: FileShareDataBus

## Cleanup strategy

> [!WARNING]
> FileShareDataBus **does not** remove physical attachments once the message has been processed.

The business requirements can specify how a message and its corresponding file should be processed and when the files can safely be removed. One strategy to deal with these attachments is to set up a cleanup policy that removes them after a specified number of days, in line with business Service-Level Agreements.

The file location used by the data bus is set at configuration time.

snippet: DefineFileLocationForDatabusFiles

This same location should be used for cleanup.

For example, this path can be used in a Handler for a message containing data bus properties.

snippet: HandlerThatCleansUpDatabus
