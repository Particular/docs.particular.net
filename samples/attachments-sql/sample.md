---
title: SQL Attachments
reviewed: 2018-01-31
component: AttachmentsSql
---

## Introduction

Uses the [SQL Attachments](/nservicebus/messaging/attachments-sql.md) project to store attachments for messages.


## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesSqlAttachments`.


## Code Walk-through

This sample uses the [Learning Transport](/transports/learning/) and the resultant messages can be viewed in the [Storage Directory](/transports/learning/#usage-storage-directory).


### Add to EndpointConfiguration

snippet: Enable

snippet: OpenConnection


### Send a message with an attachment

snippet: send

### Read an attachment for a received message

snippet: read