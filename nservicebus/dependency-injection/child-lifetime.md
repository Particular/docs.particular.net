---
title: Child containers
summary: Child containers allow for more granular instance lifetime configuration.
component: Core
reviewed: 2024-07-09

redirects:
 - nservicebus/nservicebus-support-for-child-containers
 - nservicebus/containers/child-containers
---

Child containers are a snapshot of the main container providing an additional instance lifetime scope.


## Child container scope in NServiceBus

NServiceBus creates a child container for each transport message that is received. During message processing, all `InstancePerUnitOfWork` scoped instances that are created are resolved as singletons within the context of the same message. This is helpful to support the sharing of database sessions and other resources with lifetimes specific to the processed message.

Objects can be configured in a child container scope using the scoped lifetime:

snippet: InstancePerUnitOfWorkRegistration

## Deterministic disposal

Child containers automatically dispose all [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) instances created with `InstancePerUnitOfWork` lifetime once the message is processed. This is useful for managing things like  database sessions which must be disposed of properly.
