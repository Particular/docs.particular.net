---
title: Unit of Work in NServiceBus
summary: From NServiceBus V3.0, use the IManageUnitsOfWork interface.
originalUrl: http://www.particular.net/articles/unit-of-work-in-nservicebus
tags: []
---

When using a framework like NServiceBus you usually need to create your own units of work to avoid having to repeat code in your message handlers. For example, committing NHibernate transactions, or calling `SaveChanges` on the RavenDB session.

In NServiceBus V2.6 the only hook into the message pipeline was the message modules (see [Jonathan Oliver's post](http://blog.jonathanoliver.com/2010/04/extending-nservicebus-thread-specific-message-modules/)). This had some quirks since the `HandleEndMessage` method of the message module is invoked regardless of the outcome, making it hard to decide whether to perform a commit or roll-back.

NServiceBus V3.0 introduces a new way to do this, using the `IManageUnitsOfWork` interface:


```C#
/// <summary>
/// Interface used by NServiceBus to manage units of work as a part of the
/// message processing pipeline.
/// </summary>
public interface IManageUnitsOfWork
{
    /// <summary>
    /// Called before all message handlers and modules.
    /// </summary>
    void Begin();
 
    /// <summary>
    /// Called after all message handlers and modules, if an error has occurred the exception will be passed.
    /// </summary>
    void End(Exception ex = null);
}
```

The semantics are that `Begin()` is called when the transport messages enters the pipeline. Remember that a transport message can consist of multiple application messages. This allows you to do any setup that is required. 

The `End()` method is called when the processing is complete. If there is an exception, it is passed into the method. 

This gives you a way to perform different actions depending on the outcome of the message(s).

