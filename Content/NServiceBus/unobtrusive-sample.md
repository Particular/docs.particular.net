---
title: Unobtrusive Sample
summary: Demonstrates NServiceBus operating in unobtrusive mode.
tags:
- Unobtrusive
- POCO
- Messages
- Commands
- Events
- Conventions
---

To demonstrate NServiceBus operating in unobtrusive mode, open the [unobtrusive sample](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/Unobtrusive).

Run the solution. Two console applications should start up, `Client` and `Server`.

## Configuring the Unobtrusive message

Look at the `ConventionExtensions` in the `SharedConventions` project. The code tell NServiceBus how to determine which types are message definitions by passing in your own conventions, instead of using the `IMessage`, `ICommand`, or `IEvent` interfaces:

```C#
public static void ApplyCustomConventions(this BusConfiguration busConfiguration)
{
    var conventions = busConfiguration.Conventions();
    conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
    conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
    conventions.DefiningMessagesAs(t => t.Namespace == "Messages");
    conventions.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
    conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
    conventions.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
    conventions
        .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires")
            ? TimeSpan.FromSeconds(30)
            : TimeSpan.MaxValue
        );
}
```

The code tells NServiceBus to treat all types with a namespace that ends with "Messages" the same as for messages that explicitly implement `IMessage`.

The above code instructs NServiceBus to encrypt any property that starts with the string Encrypted and resides in any class in the namespaces that ends with Command or Events, or in namespaces that are equal to Messages.

The encryption algorithm is declared in App.config of both client and server with the  `RijndaelEncryptionServiceConfig` section name. See the [Encryption](encryption.md). NServiceBus supports property level encryption by using a special `WireEncryptedString` property. The code snippet shows the unobtrusive way to tell NServiceBus which properties to encrypt.
 
It also shows the unobtrusive way to tell NServiceBus which properties to deliver on a separate channel from the message itself using the [Data Bus](attachments-databus-sample.md) feature, and which messages are express and/or have a defined time to be received.

Look at the code. There are a number of projects in the solution:

-   Client Class library sends a request and a command to the server and handles a published event
-   Server Class library handles requests and commands, and publishes events

For these three projects of message definitions, open the references to see that no references are required for NServiceBus libraries. No reference enables decoupling between those projects to NServiceBus versioning:

-   Commands Class library defines the command and definition for a returned status
-   Events Class library defines an event
-   Messages Class library defines a request and a response message, and includes messages that are express and have a time to be received

