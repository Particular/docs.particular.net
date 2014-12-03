---
title: Unit of Work in NServiceBus
summary: Implementing your own unit of work in NServiceBus.
tags: []
---

When using a framework like NServiceBus you usually need to create your own units of work to avoid having to repeat code in your message handlers. For example, committing NHibernate transactions, or calling `SaveChanges` on the RavenDB session.

To create your unit of work, implement the `IManageUnitsOfWork` interface. Here is the interface definition:

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

### Registering your unit of work

After implementing a `IManageUnitsOfWork`, you now need to register it with NServiceBus. 
Here's an example of how to register your unit of work:  

<!-- import InstancePerUnitOfWorkRegistration -->

NOTE: If you prefer, you can implement the `INeedInitialization` in the same class that implements `IManageUnitsOfWork`.

