---
title: File Share DataBus
reviewed: 2016-08-29
component: Core
tags:
 - DataBus
 - Attachments
related:
 - samples/file-share-databus
 - samples/azure/blob-storage-databus
---

The FileShare DataBus allows large properties to be transferred via a windows file share

Also [leverages both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## Usage

snippet:FileShareDataBus


## Cleanup Strategy

WARNING: FileShareDataBus **does not** remove physical attachments once the message has been processed.

The business requirements can indicate how a message and its corresponding file should be processed and when the files can safely be removed. One strategy to deal with these attachments is to set up a cleanup policy which removes any attachments after a certain number of days has passed based on business Service Level Agreements.

The file location used by the DataBus is set during configuration time.

snippet:DefineFileLocationForDatabusFiles

This same location should be used when performing the cleanup.

So for example this path can be used in a Handler for a message containing DataBus properties.

snippet:HandlerThatCleansUpDatabus