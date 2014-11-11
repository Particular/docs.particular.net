---
title: How to change the RavenDB ResourceManagerID
summary: 'Guidance on how to change the RavenDB ResourceManagerID'
tags: [RavenDB, Persistence]
---

When using RavenDB in an environment where you are relying also on distributed transactions it can happen that a commit operation fails with the following error:

> "A resource manager with the same identifier is already registered with the specified transaction coordinator"

The above is generally due to multiple RavenDB server instances, running on the same machine, trying to enlist in the same transaction with the same resource manager identifier.

It is possible to configure RavenDB to use a different resource manager identifier in two ways:

* using the `connection string` adding a `ResourceManagerId` token as in the following sample:  

```xml
<add name="myConnectionString"
          connectionString="Url=http://localhost:8080;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a" />
```

* At runtime when creating the `DocumentStore` instance:

```csharp
const string id = "d5723e19-92ad-4531-adad-8611e6e05c8a";
var store = new DocumentStore()  {
     ResourceManagerId = new Guid(id)
}
```

NOTE: Be sure that the resource manager id is constant across process restarts otherwise it will be impossibile for the instance to re-enlist in an existing distributed transaction in case of a crash.