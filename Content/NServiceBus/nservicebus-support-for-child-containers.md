---
title: NServiceBus Support for Child Containers
summary: 'Child containers are a snapshot of the main container; transient instances are treated as as singletons in the child container. '
tags: []
---

Child containers are essentially a snapshot of the main container where transient instances are treated as as singletons within the scope of the child container. This is useful when you want to scope instances for the duration of a web request or the handling of a message in NServiceBus. While this was possible before, child containers bring one more important feature to the table.

WARNING: Child containers are not supported by spring.net, so if you plan to take advantage of it, use one of the other containers supported by NServiceBus.

## Deterministic disposal

Instance lifetime is usually not tracked by the container (Windsor is an exception) and that means that you have to manually call dispose any instance that needs deterministic disposal. Child containers solve this issue by automatically disposing all transient objects created within each specific child container.

This is very handy when it comes to managing things like the database sessions session.

NServiceBus creates a child container for each transport message that is received, remembering that transport messages can contain multiple "user defined messages". This means that all transient instances created during message processing are scoped as singletons within the child container. This allows you to easily share, for example, the NHibernate session between repositories, without messing around with thread static caching.

When the message finishes processing, the child container and all transient instances are disposed. So if you need deterministic disposal, implement IDisposable.

When the message is processed, the session is disposed and all resources such as database connections are released.

Child containers are a powerful feature that can simplify your code and should definitely be in your toolbox.

If you configure your components using the NServiceBus configure API, it's possible to configure instance life-cycle to be per unit of work, using this:

#### Version 3 and 4

<!-- import InstancePerUnitOfWorkRegistrationV4 -->

#### Version 5

<!-- import InstancePerUnitOfWorkRegistrationV5 --> 
