<!--
title: "Using the In-Memory Bus"
tags: ""
summary: "<p>Until now, NServiceBus provided an asynchronous method of communication between parts of the system using the Send, Reply, and Publish API. Asynchronous forms of communication are great for ensuring reliable and durable communication between pieces of the system. Now NServiceBus 4.0 introduces the concept of an in-memory bus, applicable when events need to be handled synchronously and durability is not a concern.</p>
"
-->

Until now, NServiceBus provided an asynchronous method of communication between parts of the system using the Send, Reply, and Publish API. Asynchronous forms of communication are great for ensuring reliable and durable communication between pieces of the system. Now NServiceBus 4.0 introduces the concept of an in-memory bus, applicable when events need to be handled synchronously and durability is not a concern.


This is the same concept as for the [domain events pattern](http://www.udidahan.com/2009/06/14/domain-events-salvation/%20)
.


Events in general
-----------------


In the OO world, nouns identify objects. In the event-driven world, the word "when" identifies events. For example, if a business says, "When a customer becomes a preferred client, we want to send them a welcome email", this is the pre-cursor for the "ClientBecamePreferred" event.

In .NET 4.0, to define an event, use the event keyword in the signature of your event class, and specify the type of delegate for the event and its arguments. For example:


1.  Define an event
    
```C#
public event EventHandler<ClientBecamePreferredEventArgs> RaiseClientBecamePreferredEvent;
```


2.  Define the event arguments:
    
```C#
public class ClientBecamePreferredArgs : EventArgs
{
  public string Message {get;set;}
}

```


3.  Raise the event:
    
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



In-memory events
----------------


In-memory events are like regular .NET events in that all observing objects that have registered interest are called synchronously. They are implemented in the same fashion as the rest of the bus events. In-memory events can be useful if a few things must be handled synchronously when a certain business event has occurred. Define them as IEvent or POCOs, and use the unobtrusive conventions in the same way as before.



For example:



```C#
// Event defined using the IEvent marker interface
public class ClientBecamePreferred : IEvent
{
  // message details go here.
}

// POCO Event
public class ClientBecamePreferred
{
  // message details go here.
}
```



Read how to tell NServiceBus to [use the POCOs as events](unobtrusive-mode-messages.md) .


### How to raise an in-memory event?


In-memory events are raised using a property of an IBus object call: Bus.InMemory.Raise<t>:



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


To subscribe to these events, implement a class that implements IHandleMessages<t>. The handlers are invoked when the event is raised:



```C#
private class CustomerBecamePreferredHandler: IHandleMessages<ClientBecamePreferred>
{
  void Handle(ClientBecamePreferred message)
  {
    // Do what needs to be done when a client becomes preferred.
  }
}
```


### How is an in-memory event different from Bus.Publish<t>?


When an event is published via Bus.Publish, a message is delivered asynchronously to all of the subscribers via the queue/transport of your choice, taking into account all the messaging constraints such as the receiving party could be down, etc. Subscribers of this event can be in different machines or different endpoints on the same machine.

On the other hand, in-memory events are raised in-process and are not distributed through the bus. All registered handlers for this event are called synchronously.


### When not to use in-memory events?

Examples:

-   Where reliable and durable integration is needed; for example,
    integration with third-party web services.
-   Sending an email, because the email should be sent out only when the
    transaction succeeds in its entirety. To send out emails directly,
    use Bus.Send in the handler instead of SMTP code.

The NServiceBus style of eventing in general {dir="ltr"}
--------------------------------------------


NServiceBus uses IoC heavily. When the endpoints start, NServiceBus scans the assemblies in the directory. It finds event, command and message types, either using the marker interfaces or unobtrusive conventions. It also scans the assemblies to identify the types that implement the handlers for event types that implement IHandleMessages<t> and registers them in the container. Read more about [NServiceBus and its use of containers](containers.md) .

When an event is raised, the bus invokes the Handle method on all the registered handlers for that event. For subscribers, this offers a consistent way of subscribing to the event, regardless of how these events are published (in-memory or durable). This style of eventing has two significant advantages:


-   When new business requirements are introduced, this style of
    eventing allows the requirements to be implemented in the same
    service-oriented style of architecture for extensibility. New
    handlers can be introduced to implement the requirements along with
    the existing handlers. And by simply restarting the endpoint, the
    bus becomes aware of the new handlers. When the event is raised, the
    new handlers are invoked. This reduces the testing effort by
    implementing in such a way as to **not** touch existing working code
    to implement new functionality.
-   This style of design offers an easier way of scaling out when
    necessary. For example, to start with, the handlers can be deployed
    to the one endpoint to keep deployment small and the events raised
    using Bus.InMemory.Raise<t>. When the need to scale arises, each of
    these handlers can then be distributed to different endpoints across
    the same or different machines as needed. You can then change the
    Bus.InMemory.Raise<t> to a Bus.Publish<t> and add the necessary
    message mapping configuration in app.config for the message handlers
    in other endpoints to receive these events.


