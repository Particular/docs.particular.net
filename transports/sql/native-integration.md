---
title: SQL Transport Native Integration
summary: Sample code and scripts to facilitate native integration scenarios with SQL Server Transport.
reviewed: 2024-08-06
component: SqlTransport
related:
- samples/sqltransport/native-integration
---

This document describes how to consume messages from and send messages to non-NServiceBus endpoints via SQL Server in integration scenarios.

## Native Send

### The native send helper methods

A send involves the following actions:

* Create and serialize headers.
* Write a message body directly to SQL Server Transport.

#### In C&#35;

snippet: sqlserver-nativesend

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.

#### In PowerShell

snippet: sqlserver-powershell-nativesend

### Using the native send helper methods

snippet: sqlserver-nativesend-usage
