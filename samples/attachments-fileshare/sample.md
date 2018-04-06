---
title: FileShare Attachments
reviewed: 2018-04-06
component: AttachmentsFileShare
---

## Introduction

Uses the [FileShare Attachments](/nservicebus/messaging/attachments-fileshare.md) extension to store attachments for messages.


## Code Walk-through


### Add to EndpointConfiguration

Enable attachments for an endpoint. A directory (or network share_ and a [time to keep](/nservicebus/messaging/attachments-fileshare.md#controlling-attachment-lifetime) are required.

snippet: Enable


### Send a message with an attachment

snippet: send


### Read an attachment for a received message

snippet: read