---
title: Azure Storage Queues Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Storage Queues Transport Version 6 to 7.
reviewed: 2016-04-20
tags:
 - upgrade
 - migration
related:
- nservicebus/azure-storage-queues
- nservicebus/upgrades/5to6
---


## Azure Storage Queues Transport


### Serialization

In previous versions, the Azure Storage Queues Transport change the default `SerializationDefinition` to `JsonSerializer`.

In Version 6 of NServiceBus transports no longer have the ability to manipulate serialization. To preserve backward compatibility and ensure that message payloads are small, setting JSON serialization should now be done on the endpoint configuration level.

snippet:6to7-serializer-definition

### API Changes

In Version 7 it the public API was reduced. As a result, multiple classes that used to be public in Versions 6 and below were marked as obsolete with a comment:
*This class served only internal purposes without providing any extensibility point and as such was removed from the public API. For more information, refer to the documentation.*

If the code depends on classes that were obsoleted with the above message, and it is not clear how to update it, then [contact Particular support](http://particular.net/contactus) to get help in resolving that issue. 