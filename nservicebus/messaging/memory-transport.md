---
title: Using the In-Memory Bus
summary: The in-memory bus is applicable when events need to be handled synchronously and durability is not a concern.
tags: []
redirects:
 - nservicebus/using-the-in-memory-bus
---

NOTE: Bus.InMemory feature has been deprecated starting from Version 4.6 and removed in Version 5.0

Prior to Version 4.0, NServiceBus provided an asynchronous method of communication between parts of the system using `Send`, `Reply`, and `Publish` API. Asynchronous forms of communication are great for ensuring reliable and durable communication between parts of the system. NServiceBus Version 4.0 introduces the concept of an in-memory bus, applicable when events need to be handled synchronously and durability is not a concern.

This is the same concept as for the [domain events pattern](http://udidahan.com/2009/06/14/domain-events-salvation/).


## Events

In the OO world, nouns identify objects. In the event-driven world, the word "when" identifies events. For example, if a business rule says,
"When a customer becomes a preferred client, we want to send them a welcome email", this is the pre-cursor for the "ClientBecamePreferred" event.

In .NET 4.0, to define an event, use the event keyword in the signature of your event field, and specify the type of delegate for the event and its arguments. For example:


### Define an event
   
```C#
public event EventHandler<ClientBecamePreferredEventArgs> RaiseClientBecamePreferredEvent;
```


### Define the event arguments
   
```C#
public class ClientBecamePreferredArgs : EventArgs
{
  public string Message {get;set;}
}

```


### Raise the event
   
```C#
public void DoSomething()
{
  // Write some code that does something useful here
  // then raise the event.
  OnRaiseClientBecamePreferredEvent(new ClientBecamePreferredEventArgs("Did something"));
}

protected virtual void OnRaiseClientBecamePreferredEvent(ClientBecamePrefferedEventArgs e)
{
  EventHandler<ClientBecamePreferredEventArgs> handler = RaiseClientBecamePrefferedEvent;
  if (handler != null)
  {
    handler(this, e);
  }
}
```


## In-memory events

In-memory events are like regular .NET events in that all observing objects that have registered interest are called synchronously. They are implemented in the same fashion as the rest of the bus events. In-memory events can be useful if a few things must be handled synchronously when a certain business event has occurred. Define them as IEvent or POCOs, and use the conventions in the same way as before.

For example:


### Event defined using the IEvent marker interface

```C#
public class ClientBecamePreferred : IEvent
{
  // message details go here.
}
```


### POCO Event

```
public class ClientBecamePreferred
{
  // message details go here.
}
```

Read how to tell NServiceBus to [use the POCOs as events](/nservicebus/messaging/unobtrusive-mode.md).


### How to raise an in-memory event?

In-memory events are raised using a property of an `IBus` object call: `Bus.InMemory.Raise<T>`:

```C#
class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
  public IBus Bus { get; set; }
  public void Handle(OrderAccepted message)
  {
    // Call the domain object's action, which will in turn do the
    //below to raise the event
    Bus.InMemory.Raise<ClientBecamePreferred>(m =>
    {
      m.ClientId = message.ClientId;
    });
  }
}
```


### How to subscribe to in-memory events?

To subscribe to these events, implement a class that implements `IHandleMessages<T>`. The handlers are invoked when the event is raised:

```C#
private class CustomerBecamePreferredHandler: IHandleMessages<ClientBecamePreferred>
{
  void Handle(ClientBecamePreferred message)
  {
    // Do what needs to be done when a client becomes preferred.
  }
}
```


### How is an in-memory event different from Bus.Publish<T>?


When an event is published via Bus.Publish, a message is delivered asynchronously to all of the subscribers via the queue/transport of your choice, taking into account all the messaging constraints such as the receiving party could be down. Subscribers of this event can be in different machines or different endpoints on the same machine.

On the other hand, in-memory events are raised in-process and are not distributed through the bus. All registered handlers for this event are called synchronously.


### When not to use in-memory events?

Examples:

-   Where reliable and durable integration is needed; for example, when integrating with third-party web services.
-   Sending an email, because the email should be sent only when the transaction succeeds in its entirety. To send emails directly, use `Bus.Send` in the handler instead of SMTP code.


## NServiceBus eventing style

NServiceBus uses IoC heavily. When the endpoints start, NServiceBus scans the assemblies in the directory. It finds event, command, and message types, either the [marker interfaces or conventions](/nservicebus/messaging/messages-events-commands.md). It also scans the assemblies to identify the types that implement the handlers for event types that implement `IHandleMessages<T>`, and registers them in the container. Read more about [NServiceBus and its use of containers](/nservicebus/containers/).

When an event is raised, the bus invokes the `Handle` method on all the registered handlers for that event. For subscribers, this offers a consistent way of subscribing to the event, regardless of how these events are published (in-memory or durable).

This style of eventing has two significant advantages:

 * When new business requirements are introduced, the requirements can be implemented in the same service-oriented style of architecture, for extensibility. New handlers can be introduced to implement the requirements along with the existing handlers. By restarting the endpoint, the bus becomes aware of the new handlers. When the event is raised, the new handlers are invoked. This reduces the testing effort by implementing in such a way as to **not** touch existing working code to implement new functionality.
 * An easier way to scale out, when necessary. For example, to start with, the handlers can be deployed to the one endpoint to keep deployment small and the events raised using `Bus.InMemory.Raise<T>`. When the need to scale arises, each of these handlers can then be distributed to different endpoints across the same or different machines as needed. You can then change the `Bus.InMemory.Raise<T>` to a `Bus.Publish<T>` and add the necessary message mapping configuration in app.config for the message handlers in other endpoints to receive these events.
