---
title: File Share DataBus
summary: An implementation of databus using file shares
reviewed: 2018-04-16
component: FileShareDataBus
tags:
 - DataBus
related:
 - samples/file-share-databus
---

The file share DataBus allows large properties to be transferred via a Windows file share.

This implementation [leverages both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## Usage

snippet: FileShareDataBus


## Cleanup strategy

WARNING: FileShareDataBus **does not** remove physical attachments once the message has been processed.

The business requirements can indicate how a message and its corresponding file should be processed and when the files can safely be removed. One strategy to deal with these attachments is to set up a cleanup policy which removes any attachments after a certain number of days have passed based on business Service Level Agreements.

The file location used by the DataBus is set during configuration time.

snippet: DefineFileLocationForDatabusFiles

This same location should be used when performing the cleanup.

For example, this path can be used in a Handler for a message containing DataBus properties.

snippet: HandlerThatCleansUpDatabus
