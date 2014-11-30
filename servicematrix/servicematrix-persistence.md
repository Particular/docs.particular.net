---
title: Changing Persistence in a ServiceMatrix Project
summary: 'Selecting a persistence store within a ServiceMatrix project.'
tags:
- ServiceMatrix
- Persistence
---

NServiceBus requires a persistence store. By default, ServiceMatrix provisions your solution to use the `InMemoryPersistence` class, but only if the Visual Studio debugger is attached. If you attempt to run your project without the debugger attached, you will receive an exception informing you to choose a durable persistence class.

NServiceBus offers multiple options for different persistence technologies. Please read [Persistence In NServiceBus](../nservicebus/persistence-in-nservicebus.md) for an overview. The example below shows how to setup the RavenDB persistence store, but a similar process can be used for the other stores. 

1.  [Installing the RavenDB NuGet Package](#installing-the-ravendb-nuget-package)  
2.  [Selecting Persistence for an ASP.NET MVC Endpoint](#selecting-persistence-for-an-asp.net-mvc-endpoint)
3.  [Selecting Persistence for an NSB Host Endpoint](#selecting-persistence-for-an-nsb-host-endpoint)
4.  [Installing RavenDB 2.5](#installing-ravendb-2.5)

# Installing the RavenDB NuGet Package

Regardless of what type of endpoint you have, you will need to install the NuGet package containing the classes for the persistence store you are planning to use. Right-click on your project in the Solution Explorer and select 'Manage NuGet Packages...' Search Online for the `NServiceBus.RavenDB` package and install it.

![NServiceBus.RavenDB NuGet Package](images/servicematrix-ravendb-nuget.png)

# Selecting Persistence for an ASP.NET MVC Endpoint

For an ASP.NET MVC endpoint, you will find the setup in `Infrastructure\WebGlobalInitialization.cs`. 

<!-- import ServiceMatrix.OnlineSales.ECommerce.Infrastructure.persistence -->

Because `Infrastructure\WebGlobalInitialization.cs` is an auto-generated code file by ServiceMatrix, you should not edit it directly (or else your changes will be gone the next time it is rebuilt). Instead, add a new class file named `ConfigurePersistence.cs` to the Infrastructure folder of the ASP.NET project. Update it to initialize the RavenDBPersistence class as follows:

<!-- import ServiceMatrix.OnlineSalesV5.eCommerce.Infrastructure.ConfigurePersistence -->

# Selecting Persistence for an NSB Host Endpoint

In your NSB Host endpoints, you will find the setup in `EndpointConfig.cs`.

<!-- import ServiceMatrix.OnlineSales.OrderProcessing.EndpointConfig.before -->

Modify the code in `EndpointConfig.cs`.

<!-- import ServiceMatrix.OnlineSales.OrderProcessing.EndpointConfig.after -->

# Installing RavenDB 2.5

An NServiceBus V5 ServiceMatrix project requires RavenDB V2.5. Download the installer from [ravendb.net](http://ravendb.net/download) and select "Development" for the target environment.

NOTE: If you already have RavenDB 2.0 installed, you can uninstall the service by finding the Raven.Server.exe executable on your machine and running it from the command line with /uninstall.

For more information on installing RavenDB for use with NService bus, refer to [this document](/nservicebus/using-ravendb-in-nservicebus-installing.md).

Return to the ServiceMatrix [table of contents](./).