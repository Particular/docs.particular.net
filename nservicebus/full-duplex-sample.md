---
title: Full Duplex Sample using NServiceBus
summary: An example of full-duplex, request/response communication.
tags:
- Request Response
- Messaging Patterns
- Full Duplex
---

To see full-duplex, request/response communication, open the [Full Duplex sample](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/FullDuplex).

Run the solution. Two console applications should start up, `MyClient` and `MyServer`.

## Code walk-through

### Messages

Look at the Messages.cs file in the MyMessages project:

```C#
namespace MyMessages
{
  public class RequestDataMessage : IMessage
  {
    public Guid DataId { get; set; }
    public string String { get; set; }
  }
  
  public class DataResponseMessage : IMessage
  {
    public Guid DataId { get; set; }
    public string String { get; set; }
  }
}
```

The two classes here implement the NServiceBus IMessage interface, indicating that they are messages. The only thing these classes have are properties, each with both get and set access. The `RequestDataMessage` is sent from the client to the server, and the `DataResponseMessage` replies from the server to the client.

### Client

The client console has a input loop that does the following 

```C#
while (Console.ReadLine() != null)
{
    var g = Guid.NewGuid();

    Console.WriteLine("==========================================================================");
    Console.WriteLine("Requesting to get data by id: {0}", g.ToString("N"));

    var message = new RequestDataMessage
    {
        DataId = g,
        String = "<node>it's my \"node\" & i like it<node>"
    };
    bus.Send("Samples.DataBus.Server",message);
}
```

This code performs the following action every time the 'Enter' key is pressed:

 * A new Guid is created and then set in the outgoing headers of the bus under the "Test" key.
 * The bus sends a `RequestDataMessage `whose DataId property is set to the same `Guid`, and whose `String` property is set to an XML fragment.
 * A callback is registered and invoked when a response arrives to the request sent. In the callback, the values of several headers are written to the console.

### Server

When a RequestDataMessage arrives in the server queue, the bus dispatches it to the message handler found in the `RequestDataMessageHandler.cs` file in the MyServer project. The bus knows which classes to call, based on the interface they implement.s

```C#
public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>
```

At start up, the bus scans all assemblies and builds a dictionary indicating which classes handle which messages. So when a given message arrives in a queue, the bus knows which class to invoke.

The Handle method of this class contains this:

```C#
var response = new DataResponseMessage
{
    DataId = message.DataId,
    String = message.String
};

Bus.Reply(response);
```

Finally, the bus replies with the response message, sending it to the InputQueue specified in the MsmqTransportConfig section, in the app.config of the MyClient endpoint. The bus knows to send the responses to where the message is sent every time the bus sends a message from the queue.

When configuring the routing in the bus, continue with the premise of regular request/response communication such that clients need to know where the server is, but servers do not need to know about clients.

Look back at `ClientEndpoint.cs` to see that it gets the header information from the handler on the server.

Open `DataResponseMessageHandler.cs` in the `MyClient` project and find a class whose signature looks similar to the message handler on the server:

```C#
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
```