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


### Add to EndpointConfiguration

Enable attachments for an endpoint. A connection factory and a [time to keep](/nservicebus/messaging/attachments-sql.md#controlling-attachment-lifetime) are required.

snippet: Enable

The consuming code is responsible for opening, and handling exceptions, for a [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.aspx)

snippet: OpenConnection


### Send a message with an attachment

snippet: send


### Read an attachment for a received message

snippet: read