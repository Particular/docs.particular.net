---
title: How to change the RavenDB ResourceManagerID
summary: 'Guidance on how to change the RavenDB ResourceManagerID'
tags: [RavenDB, Persistence]
redirects:
 - nservicebus/ravendb/how-to-change-resourcemanagerid
---

WARNING: As of NServiceBus.RavenDB 3.1.0 the RavenDB ResourceManagerId is automatically set to an appropriate value, unique between all endpoints on an individual server, based on a hash of the endpoint's local address and endpoint version. It should not be necessary to manually set this value. All customers are encouraged to upgrade to at least NServiceBus.RavenDB 3.1.0 or higher. This article remains for legacy purposes only.

When using RavenDB in an environment where you are relying also on distributed transactions it can happen that a commit operation fails with the following error:

> "A resource manager with the same ID is already registered with the specified transaction coordinator"

The above is generally due to multiple RavenDB `IDocumentStore` instances that, when running on the same machine, attempt to enlist in the same transaction with matching resource manager identifiers.

When using NServiceBus with the RavenDB persistence a constant and deterministic ResourceManagerId is automatically generated for each endpoint. When configuring an endpoint with a user instantiated `IDocumentStore`, ensure that it gets a unique resource manager ID not used by any other endpoint on the same machine.

It is possible to configure RavenDB to use a different resource manager ID in two ways:

* using the `connection string` adding a `ResourceManagerId` token as in the following sample: 

```xml
<add name="NServiceBus/Persistence"
          connectionString="Url=http://localhost:8080;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a" />
```

* At runtime when creating the `DocumentStore` instance, for example when you need to inject your own document store as in the following sample:

snippet:ChangeResourceManagerID

NOTE: Be sure that the resource manager ID is constant across process restarts otherwise it will be impossible for the instance to re-enlist in an existing distributed transaction in case of a crash.
