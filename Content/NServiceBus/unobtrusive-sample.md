<!--
title: "Unobtrusive Sample"
tags: ""
summary: ""
-->

To demonstrate NServiceBus operating in unobtrusive mode, open the
[unobtrusive sample](https://github.com/NServiceBus/NServiceBus/tree/3.3.8/Samples/Unobtrusive)
.

Run the solution. Two console applications should start up. Find the client application by looking for the one with "Client" in its path and follow the onscreen instructions to send messages to the server.

Configuring the Unobtrusive message
-----------------------------------

The following code snippet shows how to determine which types are message definitions by passing in your own conventions, instead of using the IMessage, ICommand, or IEvent interfaces:


```C#
Configure.With()
  .DefaultBuilder()
  .FileShareDataBus(@"\\MyDataBusShare\")
  .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
  .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
  .DefiningMessagesAs(t => t.Namespace == "Messages")
  .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
  .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
  .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
  .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") 
  ? TimeSpan.FromSeconds(30) 
  : TimeSpan.MaxValue);
```

 The code tells NServiceBus to treat all types with a namespace that ends with "Messages" the same as for messages that explicitly implement IMessage.

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


```C#
.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
.DefiningMessagesAs(t => t.Namespace == "Messages")
```

 The code tells NServiceBus to treat classes that are declared in the Messages namespace as if they explicitly implement the IMessage interface. Any type in a namespace ending with "Events" is treated as if it explicitly implements the IEvent, and any type in a namespace ending with "Commands" is treated as if it explicitly implements the ICommand.

Open the client application configuration file (app.config) to see its configuration:


```XML
<add Messages="Messages" Endpoint="server"/>
```

 The above declaration instructs NServiceBus that the target of IMessage type messages is the Server endpoint.

Following is the client code to send the Request:


```C#
Bus.Send<Request>(m =>
{
  m.RequestId = requestId;
});
```


### Server side

The server side handles the Request in the RequestMessageHandler class and only replies to the client as follows:


```C#
Bus.Reply(new Response
{
  ResponseId = message.RequestId
});
```

 In the client is the Response handler code:


```C#
public class ResponseHandler : IHandleMessages<Response>
```

 Command/Status messaging pattern code
-------------------------------------

### Client side message declaration

The following declaration instructs NServiceBus to use classes with a namespace ending with Commands in the same way as for messages that explicitly implement the ICommand interface:


```C#
.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
```

 Open the client application configuration file (app.config) to see its configuration:


```XML
<add Messages="Messages" Endpoint="server"/>
```

 The above declaration instructs NServiceBus that the target of ICommand type messages is the Server endpoint.

Following is the client code to send the Command:


```C#
Bus.Send<MyCommand>(m =>
{
  m.CommandId = commandId;
  m.EncryptedString = "Some sensitive information";
}).Register<CommandStatus>(outcome=> Console.WriteLine("Server returned status: " + outcome));
```

 The client sends a message and registers a method to handle the returned status.

### Server side

The server side handles the Handle method. It simply returns an OK status:


```C#
public class MyCommandHandler : IHandleMessages<MyCommand>
{
  readonly IBus bus;
  public MyCommandHandler(IBus bus)
  {
    this.bus = bus;
  }
  public void Handle(MyCommand message)
  {
    Console.WriteLine("Command received, id:" + message.CommandId);
    Console.WriteLine("EncryptedString:" + message.EncryptedString);
    bus.Return(CommandStatus.Ok);
  }
}
```

 The message.EncryptedString is encrypted by the NServiceBus framework since it was declared as shown in the endpointConfig class (of both client and server):


```C#
Configure.With()
  .DefaultBuilder()
  .FileShareDataBus(@"\\MyDataBusShare\")
  .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
  .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
  .DefiningMessagesAs(t => t.Namespace == "Messages")
  .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
  .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
  .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
  .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") 
  ? TimeSpan.FromSeconds(30) 
  : TimeSpan.MaxValue);
```

 The above code instructs NServiceBus to encrypt any property that starts with the string Encrypted and resides in any class in the namespaces that ends with Command or Events, or in namespaces that are equal to Messages.

The encryption algorithm is declared in App.config of both client and server with the RijndaelEncryptionServiceConfig section name. See the
[Encryption sample](encryption-sample.md) .

Publish/Subscribe messaging pattern code
----------------------------------------

For a complete sample, see the [Pub/Sub documentation](how-pub-sub-works.md) .

### Client side m <span style="font-size: 14px;">essage declaration</span>

The following declaration instructs NServiceBus to use those classes with a namespace that ends with Events in the same way as for messages that explicitly implement the IEvent interface:


```C#
.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
```

 Open the client application configuration file (app.config) to see its configuration:


```XML
<add Messages="Messages" Endpoint="server"/>
```

 The above declaration instruct NServiceBus that the target of IEvent type messages is the Server endpoint. For events, it means that the client subscribes to the publishing Server endpoint.

Following is the client code to handle published events and print a message to the console:


```C#
public class MyEventHandler : IHandleMessages<IMyEvent>
{
  public void Handle(IMyEvent message)
  {
    Console.WriteLine("IMyEvent received from server with id:" + message.EventId);
  }
}
```


### Server side

On the server side, it publishes an event, as follows:


```C#
Bus.Publish<IMyEvent>(m =>
{
  m.EventId = eventId;
});
```

 When using naming convention to mark your commands events and messages, you can achieve freedom from dependency on NServiceBus message versioning. The sample shows that after declaring messages, commands, and events, the way NServiceBus sends and receives code is identical to scenarios where the messages interface implementation is done explicitly.

Next steps
----------

Read about [Message Mutators](nservicebus-message-mutators-sample.md) .

