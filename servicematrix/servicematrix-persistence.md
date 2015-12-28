---
title: Changing Persistence in a ServiceMatrix Project
summary: 'Selecting a persistence store within a ServiceMatrix project.'
tags:
- ServiceMatrix
- Persistence
---

include: sm-discontinued

NServiceBus requires a persistence store. By default, ServiceMatrix provisions your solution to use the `InMemoryPersistence` class, but only if the Visual Studio debugger is attached. If you attempt to run your project without the debugger attached, you will receive an exception informing you to choose a durable persistence class.

NServiceBus offers multiple options for different persistence technologies. Please read [Persistence In NServiceBus](/nservicebus/persistence/) for an overview. The example below shows how to setup the RavenDB persistence store, but a similar process can be used for the other stores.

# Installing the RavenDB NuGet Package

Regardless of what type of endpoint you have, you will need to install the NuGet package containing the classes for the persistence store you are planning to use.

Open the NuGet Package Manager Console: `Tools > NuGet Package Manager > Package Manager Console`.

Type the following command at the Package Manager Console:

    PM> Install-Package NServiceBus.RavenDB

NOTE: When prompted to reload the project, click reload

# Selecting Persistence for an ASP.NET MVC Endpoint

For an ASP.NET MVC endpoint, you will find the setup in `Infrastructure\WebGlobalInitialization.cs`.

````C#
if (Debugger.IsAttached)
{
    // For production use, please select a durable persistence.
    // To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
    // To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();   
    config.UsePersistence<InMemoryPersistence>();
...
````

Because `Infrastructure\WebGlobalInitialization.cs` is an auto-generated code file by ServiceMatrix, you should not edit it directly (or else your changes will be gone the next time it is rebuilt). Instead, add a new class file named `ConfigurePersistence.cs` to the Infrastructure folder of the ASP.NET project. Update it to initialize the RavenDBPersistence class as follows:

````C#
using NServiceBus;
using NServiceBus.Persistence;

namespace OnlineSalesV5.eCommerce.Infrastructure
{
    public class ConfigurePersistence : INeedInitialization
    {
        public void Customize(BusConfiguration config)
        {
            config.UsePersistence<RavenDBPersistence>();
        }
    }
}
````

# Selecting Persistence for an NSB Host Endpoint

In your NSB Host endpoints, you will find the setup in `EndpointConfig.cs`.

````C#
// For production use, please select a durable persistence.
// To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
// To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();
if (Debugger.IsAttached)
{
    configuration.UsePersistence<InMemoryPersistence>();
}
````

Modify the code in `EndpointConfig.cs`.

````C#
using NServiceBus;
using NServiceBus.Persistence;

namespace OnlineSales.OrderProcessing
{
    public partial class EndpointConfig   
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<RavenDBPersistence>();
        }
    }
}
````

# Installing RavenDB

Refer to [Installing RavenDB](/nservicebus/ravendb/installation.md) for installation instructions.
