---
title: FileShare Attachments
summary: Use a file share to store attachments for messages.
reviewed: 2018-04-06
component: AttachmentsFileShare
related:
 - samples/attachments-fileshare
 - samples/attachments-sql
---

Uses a file share to store attachments for messages.


## Usage

Two settings are required as part of the default usage:

 * A file share or directory location.
 * A default time to keep for attachments.

snippet: EnableAttachments


### Recommended Usage

Uses the `NServiceBus.Attachments.Sql.TimeToKeep.Default` method for attachment cleanup.

This usage results in the following:

snippet: EnableAttachmentsRecommended


include: attachments