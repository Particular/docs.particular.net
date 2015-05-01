---
title: Installing RavenDB
summary: Article outlines various ways of installing RavenDB in different versions of NServiceBus. As of version 5 it is mostly obsolete due to the fact that RavenDB is no longer part of the core.
tags:
- Persistence
- RavenDB
redirects:
 - nservicebus/using-ravendb-in-nservicebus-installing
---

Starting from NServiceBus version 5.0 RavenDB is no longer the default persistence option in NServiceBus. A user has to explicitly select persistence. For more information, on the various available persistence options, please read: [Persistence in NServiceBus](/nservicebus/persistence/).

RavenDB-related code has been moved to a separate package  [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) and it is up to a user to install the RavenDB server from the [official download page](http://ravendb.net/download) following instructions on RavenDB website.

NServiceBus version 4.x and 3.x used RavenDB for persistence by default. The NServiceBus license permits the use of RavenDB for the storage needs of your endpoint. This only includes NServiceBus-related data such as sagas and subscriptions. If you store application-specific data in RavenDB you need to purchase a separate license.

To use the default storage, ensure you have a RavenDB server running to which your endpoint can connect. To set up RavenDB [Download](http://ravendb.net/download) and install it yourself.

## NServiceBus auto-installation of RavenDB

**Versions 5.x:** RavenDB is no longer auto-installed.

**Versions 4.x:** The /installinfrastructure has been deprecated in version 4.0 and above on NServiceBus.Host, and NServiceBus now expects RavenDB instance to be pre-installed and accessible to it. Use the [powershell commandlets](/nservicebus/operations/management-using-powershell.md) to install RavenDB on the needed servers. When the endpoints starts up, if the host is configured for RavenDB persistence and if the configured RavenDB persistence could not be contacted, then warnings will be logged.

**Versions 3.x:** Note: These versions of NServiceBus.RavenDB are only due to be available on the first week of May 2015. 
RavenDB is included with the NServiceBus binaries (not ilmerged). When using the `/installinfrastructure` switch on the NServiceBus.Host at install time, the infrastructure installer ensures that RavenDB is installed on the local machine. The install is only performed if the following is true:

-   RavenDB persistence is configured for the endpoint
-   A custom connection string is not specified by the user
-   The current endpoint has no master node specified
-   Port 8080 is available
-   RavenDB is not already installed

\*When a master node is defined, NServiceBus understands that all data will be stored on that designated server  instead. This means that the RavenDB server runs on that remote machine and not the local machine where you run the install.

For instructions on how to administrate and operate a RavenDB server, refer to the [documentation for RavenDB](http://ravendb.net/docs/article-page/2.5/csharp/server/administration).

## Upgrading RavenDB

To upgrade an existing RavenDB installation refer to the [RavenDB website article describing the upgrade process](http://ravendb.net/docs/article-page/2.0/csharp/server/administration/upgrade).

Note it is highly recommended that you backup your Raven database prior to upgrading.

## Which versions of RavenDB are compatible?

For a more detailed overview over the compatibility to RavenDB see also the [version compatibility](/nservicebus/ravendb/version-compatibility.md) documentation.

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) greater or equal V3.0.0 requires RavenDB V3.0 build 3660 or higher build number for version 3.0.

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) smaller than V3.0.0 requires RavenDB v2.5 build 2908 or higher build number for version 2.5.

NOTE: If you already have RavenDB 2.0 installed from a previous NServiceBus V4.0 or prior version and want to run v2.5, you can uninstall the service by finding the Raven.Server.exe executable on your machine and running it from the command line with /uninstall. [Read this document](uninstalling-v4.md) for full removal instructions. Note that the v2.5 download will be unlicensed, so you will need to either copy the license.xml file from the v2.0 installation folder to the v2.5 installation folder, contact Particular Software and request a v2.5 license file, or use your own license file.  

NServiceBus V4.0 is tested and compatible with RavenDB version 2261 and RavenDB v2.

NServiceBus V3.X is tested and compatible with all RavenDB versions from 616 through 992 and RavenDB v1. We strongly recommend using 992 since it has better support for transaction recovery. Download v992 here: [RavenDB Server - 992](http://hibernatingrhinos.com/builds/ravendb-stable-v1.0/992).

For more information regarding RavenDB compatibility, please refer to [this article](version-compatibility.md) and the [RavenDB website](http://ravendb.net/docs/article-page/2.0/csharp/client-api/backward-compatibility).
