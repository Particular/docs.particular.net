---
title: Child lifetime
summary: Child lifetimes are a snapshot of the main instance; transient instances are treated as as singletons in the child lifetime.
component: Core
reviewed: 2017-10-06
redirects:
 - nservicebus/nservicebus-support-for-child-containers
 - nservicebus/containers/child-containers
---

Child lifetimes are essentially a snapshot of the main instance where transient instances are treated as singletons within the scope of the child lifetime. This is useful to scope instances for the duration of a web request or the handling of a message in NServiceBus. While this was possible before, child lifetimes bring one more important feature to the table.

NOTE: As of [NServiceBus.Spring](https://www.nuget.org/packages/NServiceBus.Spring) Version 6.0.0, child lifetimes are now supported as well. Previous versions don't support child lifetimes so one of the other containers supported by NServiceBus must be used to take advantage of it.


## Deterministic disposal

Instance lifetime is usually not tracked by the main instance (Windsor is an exception) and that means that manually disposing is required on any instance that needs deterministic disposal. Child lifetimes solve this issue by automatically disposing all transient objects created within each specific child lifetime.

This is useful to managing things like the database sessions.

NServiceBus creates a child lifetime for each transport message that is received, remembering that transport messages can contain multiple "user defined messages". This means that all transient instances created during message processing are scoped as singletons within the child lifetime. This supports easily sharing, for example, the NHibernate session between repositories, without messing around with thread static caching.

When the message finishes processing, the child lifetime and all transient instances are disposed. So if deterministic disposal is required, implement [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx).

When the message is processed, the session is disposed and all resources such as database connections are released.

If configuring components using the NServiceBus configure API, it's possible to configure instance life-cycle to be per unit of work, using this:

snippet: InstancePerUnitOfWorkRegistration
