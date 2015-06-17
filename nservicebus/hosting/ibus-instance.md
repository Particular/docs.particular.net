---
title: Managing IBus instance
summary: Fluent configuration API to get transnational one-way messaging
tags:
- Hosting
- IOC
- Dependency Injection
- Service Locator
---


When you have created a `IBus` instances then you must register it somewhere so that your application can use it to send messages.

There are various ways in managing and storing the `IBus` instance.

* Global static
* Using dependency injection

The first option creates tight coupling the last loose coupling.



## Global static

After you have created the bus you assign the object to a global static variable so that the bus can be used in other locations


Initializing:

```
public class Global
{
    public static IBus Bus { get; set; }
}

Global.Bus = NServiceBus.Create(busConfiguration).Start();
```

Using it:

```
Global.Bus.Send(new MyCommand());
```

Cleanup:

```
Global.Bus.Dispose();
```

This is very simple to use but creates a lot of tight coupling which makes unit testig pretty hard.

## Dependency injection container

Here is the description of the dependency injection pattern:

>In software engineering, dependency injection is a software design pattern that implements inversion of control for software libraries. Caller delegates to an external framework the control flow of discovering and importing a service or software module specified or "injected" by the caller.

Source: https://nl.wikipedia.org/wiki/Dependency_injection


### Registering

When you have created the `IBus` instance then you need to register it in a dependency injection container so that it knows what and how it can inject it into objects that require an `IBus` instance.

### Single instance

The following example uses Autofac but looks very similar with other frameworks


Registering bus instance:
```
ContainerBuilder builder = new ContainerBuilder();
builder.RegisterInstance(bus);
IContainer container = builder.Build();
```

This will make sure that when a object is created via the container and it is dependent on `IBus` that this instance will be injected.

NServiceBus will still use its own container to resolve its own objects. NServiceBus can also use an external container.

See: [Containers](/nservicebus/containers/index.md)

You need to use this when you want types that are registered in the external controller to be injected in to objects created by NServiceBus like message handlers and message mutators.



### Disposing

When the application done it only needs to dispose the container. The container should take care of disposing all objects in the correct order.

There should be no need in calling `IBus.Dispose` when you have registered the bus instance in the container.

NOTE: Some containers will not dispose registered instances unless explicitly configured to do so.

## Custom container

In t

Dependency Injection is used internally by NServiceBus and is referred to as the 'object builder'. Sometimes yo



Samples 

http://docs.particular.net/samples/web/asp-mvc-injecting-bus/
http://docs.particular.net/nservicebus/containers/
http://docs.particular.net/samples/containers/





