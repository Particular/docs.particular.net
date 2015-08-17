---
title: Unit of Work For RavenDB
summary: Avoid repeating code in your message handlers by implementing the NServiceBus Unit of Work for RavenDB.
tags: []
redirects:
 - nservicebus/unit-of-work-implementation-for-ravendb
---

When using a framework like NServiceBus many times you may need to create your own unit of work to avoid repeating code in your message handlers. Following is one approach on how to implement the NServiceBus Unit of Work for RavenDB.

## Sharing the session

In order to share the RavenDB session between the message handler(s), it is best to share it through a 'unit of work' implementation. It is recommended NOT to use thread static variables, which are not cleared when the request ends, and may cause various connectivity or corrupted data problems in the next request, for example if another type of message was handled between the handling of two handlers for a subscribed event. 

Instead, beginning with NServiceBus version 3, use the new [support for child containers](/nservicebus/containers/child-containers.md). This means that all dependencies configured as a single call effectively become static within the context of one transport message, and thus can be shared by that message's handlers, but the next message will have cleared and renewed the information.

To resolve a RavenDB document session from the container, add the following configuration (StructureMap is used but any of the other containers except Spring and Unity would work):  

```C#
// Note: The 'store' variable's contents will become statically available to message hanlers
// through injection by the container and the 'Unit of Work' mechanism 
var store = new DocumentStore {Url = "http://localhost:8080", DefaultDatabase = "MyDatabase"};
store.Initialize();
 
Container container = new Container( c => 
                    {
                        c.For<IDocumentStore>().Singleton()
                            .Use(dbStore);
                        c.For<IDocumentSession>()
                            .Use(ctx => ctx.GetInstance<IDocumentStore>()
                                        .OpenSession());
                        c.For<IManageUnitsOfWork>()
                            .Use<RavenUnitOfWork>();
                    });
configuration.UseContainer<StructureMapBuilder>(c =>
                       c.ExistingContainer(container));

// rest of contiguration goes here... for example:
configuration.UsePersistence<RavenDBPersistence>();


```

The above code tells the container to create a new `IDocumentSession` using the specified lambda. The fact that all message processing is done using a child container means that all message handlers processing the message get the same session instance.

If using the NServiceBus host via Nuget, you can add the above code inside the Customize() method in the EnpointConfig. You must use Nuget for the relevant container (i.e. NServiceBus.StructureMap in this example). See the updated sample code for a full implementation. 


***IMPORTANT: When using the Unit-of-Work NServiceBus detects that you are using the RavenDB license for work other than the NServiceBus persistance and requires a separate license.***
In order to overcome this, either ask for the license from support at Particular, or use without a unit-of-work, as explained at the bottom of this article.

## Implementing the unit of work

In RavenDB, to persist your data to the database, you need to explicitly call `IDocumentSession.SaveChanges()`. To avoid making this call in all the handlers, add a unit of work implementation. This saves typing and prevents you from forgetting to make the call:


```C#
public class RavenUnitOfWork : IManageUnitsOfWork
{
    private readonly IDocumentSession session;
 
    public RavenUnitOfWork(IDocumentSession session)
    {
        this.session = session;
    }
 
    public void Begin()
    {
    }
 
    public void End(Exception ex)
    {
        if (ex == null)
        {
            session.SaveChanges();
        }
    }
}
```

NOTE: There is a dependency on the `IDocumentSession`. Given that the UoW is resolved from the same child container as the handlers, you will get the same session instance. RavenDB doesn't need any special setup so you only need to call `SaveChanges` if `End()` is called and no exception occurs.

To make NServiceBus use the UoW, configure it in the container so that NServiceBus finds and uses it:

```C#
c.For<IManageUnitsOfWork>()
    .Use<RavenUnitOfWork>();
```

## Disposing of the session

Rescue comes from the child containers together with the fact that the main container disposes of all single call components created in the child container together with the child container. NServiceBus disposes of the child container when it finishes processing a transport message, which means that any object implementing `IDisposable` is disposed of. Luckily, [IDocumentSession](https://github.com/ravendb/ravendb/blob/master/Raven.Client.Lightweight/IDocumentSession.cs) does just this! So it is possible to create clean message handlers that interact with Raven:

```C#
public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    readonly IDocumentSession session;
 
    public PlaceOrderHandler(IDocumentSession session)
    {
        this.session = session;
    }
 
    public void Handle(PlaceOrder message)
    {
        session.Store(new Order
        {
            OrderId = message.OrderId
        });
    }
}
```

## Working code, please!

A [working sample](https://github.com/andreasohlund/Blog/tree/master/RavenUnitOfWork) is at [Andreas Öhlund's github account](https://github.com/andreasohlund/) . 

**NOTES** :

-The solution automatically downloads the required dependencies using NuGet.
-The sample assumes RavenDB is listening on `http://localhost:8080`.

## Static instance - the Old way without Unit-of-Work
If for any reason you cannot use the Unit-of-Work, i.e. you wish to use the development license supplied by RavenDB, you can use a Staticly supplied datastore although it has several issues, as mentioned above and described by [Andreas Öhlund](http://andreasohlund.net/) in his [blog post](http://andreasohlund.net/2010/03/25/thread-static-caching-in-nservicebus/). Following is the code for a static RavenDB Datastore property:  

```C#
// in the EndpointConfig.cs class: 
using Raven.Client;
using Raven.Client.Document;

private static DocumentStore dbStore;
public static DocumentStore DbStore
{
   get
   {
       if (dbStore != null)
           return dbStore;
       dbStore = new DocumentStore{ ConnectionStringName = "cnnMyConnectionString" };
       dbStore.Initialize();
       return dbStore;
    }
 }

```
You can then use it in your handler or saga code thus:

```C#
// inside Handle() function...
using ( var session = EndpointConfig.DbStore.OpenSession() )
{
     // session.Store(...  etc. 
     session.SaveChanges();
}
```
