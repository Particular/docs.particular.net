---
title: Unobtrusive
summary: Demonstrates NServiceBus operating in unobtrusive mode.
reviewed: 2016-03-21
component: Core
tags:
- Unobtrusive
- Command
- Event
- Convention
redirects:
- nservicebus/unobtrusive-sample
related:
- nservicebus/messaging/unobtrusive-mode
---

Run the solution. Two console applications should start up, `Client` and `Server`.


## Configuring the Unobtrusive message

Look at the `ConventionExtensions` in the `SharedConventions` project. The code tells NServiceBus how to determine which types are message definitions by passing in custom conventions, instead of using the `IMessage`, `ICommand`, or `IEvent` interfaces:

snippet: CustomConvention

The code tells NServiceBus to treat all types with a namespace that ends with "Messages" the same as for messages that explicitly implement `IMessage`.

The above code instructs NServiceBus to encrypt any property that starts with the string Encrypted and resides in any class in the namespaces that ends with Command or Events, or in namespaces that are equal to Messages.

The encryption algorithm is declared in `app.config` of both client and server with the  `RijndaelEncryptionServiceConfig` section name. See the [Message Property Encryption](/nservicebus/security/property-encryption.md). NServiceBus supports property level encryption by using a special `WireEncryptedString` property. The code snippet shows the unobtrusive way to tell NServiceBus which properties to encrypt.

It also shows the unobtrusive way to tell NServiceBus which properties to deliver on a separate channel from the message itself using the [Data Bus](/nservicebus/messaging/databus/) feature, and which messages have a defined time to be received.

Look at the code. There are a number of projects in the solution:

 * **Client**: sends a request and a command to the server and handles a published event
 * **Server**: handles requests and commands, and publishes events
 * **Shared**: Shared code including the message conventions and message definitions.