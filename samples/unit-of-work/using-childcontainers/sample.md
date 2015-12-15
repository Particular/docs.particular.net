---
title: Unit of work using Child Containers
summary: How to create a unit of work that relies on child container support to store the session
tags:
- Container
- UnitOfWork
- RavenDB
- Child Container
- StructureMap
---


## Running the sample

Run the Solution.  Hit enter a few times and notice that an order will be stored for each key press. You can then browse the url presented by the sample to verify the data that gets stored in RavenDB.


## Code walkthrough

This sample shows how to share a RavenDB `IDocumentSession` between multiple handlers and finally call `.SaveChanges` once per message. This has the following benefits:

1. Applies the changes in one round trip to the database
2. Avoids the need to call `.SaveChanges` in all handlers

With the UoW in place our handlers can focus solely on the business problem we're solving. Our handler in the sample looks like this:

snippet:PlaceOrderHandler

Notice how we inject the `IDocumentSession` and then call `.Store` on it.


### Saving the changes

With RavenDB you need to call `.SaveChanges` explicitly. We'll add a pipeline step that does this once after all our handlers have been called.

In this sample we'll use a incoming pipeline behavior to handler this.

snippet:RavenUnitOfWork

Notice how we first call `next()` to invoke the pipeline and if no exception is thrown we'll call `.SaveChanges()` to apply the changes to our database.


### Registering the UoW in the pipeline

This is a easy as

snippet:PipelineRegistration


### Sharing the session between handlers and behavior

For this we're leaning on the child container support of our container. This sample is using StructureMap but it would work on most other containers as well. (please check the documentation for your chosen container to make sure)

NServiceBus will create a child container per message and the semantics of a child container is that any instances created by it becomes static for the lifetime of that container. This is exactly what we need since that means that it will be created when the pipeline step is execute and the same instance will be injected into our handlers.

The final piece of the puzzle is disposing of the session. Again the works out of the box since the child container will automatically dispose any instances that was created by it.

With this in mind we configure our StructureMap container as follows

snippet:ContainerConfiguration

Notice how we define the session factory as `single instance` and the session as `per call`.



### In Process Raven Host

So that no running instance of RavenDB server is required.

snippet:ravenhost
