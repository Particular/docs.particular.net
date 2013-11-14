<!--
title: "Unobtrusive Sample"
tags: ""
summary: "<p>To demonstrate NServiceBus operating in unobtrusive mode, open the
<a href="https://github.com/NServiceBus/NServiceBus/tree/3.3.8/Samples/Unobtrusive">unobtrusive sample</a>
.</p>
<p>Run the solution. Two console applications should start up. Find the client application by looking for the one with &quot;Client&quot; in its path and follow the onscreen instructions to send messages to the server.</p>
"
-->

To demonstrate NServiceBus operating in unobtrusive mode, open the
[unobtrusive sample](https://github.com/NServiceBus/NServiceBus/tree/3.3.8/Samples/Unobtrusive)
.

Run the solution. Two console applications should start up. Find the client application by looking for the one with "Client" in its path and follow the onscreen instructions to send messages to the server.

Configuring the Unobtrusive message
-----------------------------------

The following code snippet shows how to determine which types are message definitions by passing in your own conventions, instead of using the IMessage, ICommand, or IEvent interfaces:

<script src="https://gist.github.com/Particular/6115991.js?file=UnobtrusiveSample.cs"></script> The code tells NServiceBus to treat all types with a namespace that ends with "Messages" the same as for messages that explicitly implement IMessage.

You can also specify conventions for the new [ICommand and IEvent feature](introducing-ievent-and-icommand.md) .

NServiceBus supports property level encryption by using a special WireEncryptedString property. The code snippet shows the unobtrusive way to tell NServiceBus which properties to encrypt. It also shows the unobtrusive way to tell NServiceBus which properties to deliver on a separate channel from the message itself using the [Data Bus](attachments-databus-sample.md) feature, and which messages are express and/or have a defined time to be received.

Look at the code. There are a number of projects in the solution:

-   Client Class library sends a request and a command to the server and
    handles a published event
-   Server Class library handles requests and commands, and publishes
    events

For these three projects of message definitions, open the references to see that no references are required for NServiceBus libraries. No reference enables decoupling between those projects to NServiceBus versioning:

-   Commands Class library defines the command and definition for a
    returned status
-   Events Class library defines an event
-   Messages Class library defines a request and a response message, and
    includes messages that are express and have a time to be received

Sample messaging patterns
-------------------------

This sample contains three messaging patterns:

-   Full Duplex: Also known as Send/Reply, the client sends a request
    and handles a response that was replied to by the server
-   Command: The client sends a command to the server that returns a
    status
-   Messages Class library: defines a request and a response message.
    See that no references are required for NServiceBus libraries

Send/Reply messaging pattern code
---------------------------------

Read the [Full Duplex](full-duplex-sample-v3.md) sample.

### Client side message declaration

The following code in the client project endpointConfig configures the messages:

<script src="https://gist.github.com/Particular/6115991.js?file=ClientSideDeclaration.cs" < script>

<p>The code tells NServiceBus to treat classes that are declared in the Messages namespace as if they explicitly implement the IMessage interface. Any type in a namespace ending with "Events" is treated as if it explicitly implements the IEvent, and any type in a namespace ending with "Commands" is treated as if it explicitly implements the ICommand.</p>

<p>Open the client application configuration file (app.config) to see its configuration:</p>
<script src="https://gist.github.com/Particular/6115991.js?file=MessageMapping.xml"></script> The above declaration instructs NServiceBus that the target of IMessage type messages is the Server endpoint.

Following is the client code to send the Request:

<script src="https://gist.github.com/Particular/6115991.js?file=ClientCode.cs"></script>
### Server side

The server side handles the Request in the RequestMessageHandler class and only replies to the client as follows:

<script src="https://gist.github.com/Particular/6115991.js?file=ServerCode.cs"></script> In the client is the Response handler code:

<script src="https://gist.github.com/Particular/6115991.js?file=Handler.cs"></script> Command/Status messaging pattern code
-------------------------------------

### Client side message declaration

The following declaration instructs NServiceBus to use classes with a namespace ending with Commands in the same way as for messages that explicitly implement the ICommand interface:

<script src="https://gist.github.com/Particular/6115991.js?file=DefineCommand.cs"></script> Open the client application configuration file (app.config) to see its configuration:

<script src="https://gist.github.com/Particular/6115991.js?file=MessageMapping.xml"></script> The above declaration instructs NServiceBus that the target of ICommand type messages is the Server endpoint.

Following is the client code to send the Command:

<script src="https://gist.github.com/Particular/6115991.js?file=SendMyCommand.cs"></script> The client sends a message and registers a method to handle the returned status.

### Server side

The server side handles the Handle method. It simply returns an OK status:

<script src="https://gist.github.com/Particular/6115991.js?file=MyCommandHandler.cs"></script> The message.EncryptedString is encrypted by the NServiceBus framework since it was declared as shown in the endpointConfig class (of both client and server):

<script src="https://gist.github.com/Particular/6115991.js?file=UnobtrusiveSample.cs"></script> The above code instructs NServiceBus to encrypt any property that starts with the string Encrypted and resides in any class in the namespaces that ends with Command or Events, or in namespaces that are equal to Messages.

The encryption algorithm is declared in App.config of both client and server with the RijndaelEncryptionServiceConfig section name. See the
[Encryption sample](encryption-sample.md) .

Publish/Subscribe messaging pattern code
----------------------------------------

For a complete sample, see the [Pub/Sub documentation](how-pub-sub-works.md) .

### Client side m <span style="font-size: 14px;">essage declaration</span>

The following declaration instructs NServiceBus to use those classes with a namespace that ends with Events in the same way as for messages that explicitly implement the IEvent interface:

<script src="https://gist.github.com/Particular/6115991.js?file=DefineEvent.cs"></script> Open the client application configuration file (app.config) to see its configuration:

<script src="https://gist.github.com/Particular/6115991.js?file=MessageMapping.xml"></script> The above declaration instruct NServiceBus that the target of IEvent type messages is the Server endpoint. For events, it means that the client subscribes to the publishing Server endpoint.

Following is the client code to handle published events and print a message to the console:

<script src="https://gist.github.com/Particular/6115991.js?file=MyEventHandler.cs"></script>
### Server side

On the server side, it publishes an event, as follows:

<script src="https://gist.github.com/Particular/6115991.js?file=PublishEvent.cs"></script> When using naming convention to mark your commands events and messages, you can achieve freedom from dependency on NServiceBus message versioning. The sample shows that after declaring messages, commands, and events, the way NServiceBus sends and receives code is identical to scenarios where the messages interface implementation is done explicitly.

Next steps
----------

Read about [Message Mutators](nservicebus-message-mutators-sample.md) .

