---
title: Child containers
summary: Child containers allow for more granular instance lifetime configuration.
component: Core
reviewed: 2019-07-08
redirects:
 - nservicebus/nservicebus-support-for-child-containers
 - nservicebus/containers/child-containers
---

Child containers are a snapshot of the main container providing an additional instance lifetime scope.


## Child container scope in NServiceBus

NServiceBus creates a child container for each transport message that is received. This means that during message processing, all `InstancePerUnitOfWork` scoped instances that are created are scoped as singletons within the context of processing each message. This is helpful to support the sharing of database sessions and other resources with lifetimes specific to the processed message.

Objects can be configured in a child container scope using the `InstancePerUnitOfWork` lifetime:

snippet: InstancePerUnitOfWorkRegistration


## Deterministic disposal

Instance lifetime is usually not tracked by the main instance (Windsor is an exception) and that means that manual disposal is required on any instance that needs deterministic disposal. Child containers solve this issue by automatically disposing all instances created within each specific child lifetime. This is useful to managing things like the database sessions.

When the message finishes processing, the child lifetime and all associated instances implementing [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) are disposed.
