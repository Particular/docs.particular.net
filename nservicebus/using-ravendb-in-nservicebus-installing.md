---
title: Installing RavenDB for NServiceBus
summary: Article outlines various ways of installing RavenDB in different versions of NServiceBus. As of version 5 it is mostly obsolete due to the fact that RavenDB is no longer part of the core.
tags:
- Persistence
- RavenDB
---

Starting from NServiceBus version 5.0 RavenDB is no longer a default peristence mechanism in NServiceBus. A user has to explicitly select either RavenDB or NHibernate. RavenDB-related code has been moved to a separate package [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) and it is up to a user to install the RavenDB server form the [official download page](http://ravendb.net/download) following instructions on RavenDB website.

Starting from version 3.0, including all 3.x and 4.x releases, NServiceBus used RavenDB for persistence by default. The NServiceBus license permits the use of RavenDB for the storage needs of your endpoint. This only includes NServiceBus-related data such as sagas and subscriptions. If you store application-specific data in RavenDB you need to purchase a separate license.

To use the default storage, ensure you have a RavenDB server running to which your endpoint can connect. There are two ways to set it up:

-   [Download](http://ravendb.net/download) and install RavenDB yourself (if you already use Raven or if you have other specific requirements for the installation)
-   Let NServiceBus do it for you

## NServiceBus auto-installation of RavenDB

**Versions 5.x:** RavenDB is no longer auto-installed.

**Versions 4.x:** The /installinfrastructure has been deprecated in version 4.0 and above on NServiceBus.Host, and NServiceBus now expects RavenDB instance to be pre-installed and accessible to it. Use the [powershell commandlets](managing-nservicebus-using-powershell.md) to install RavenDB on the needed servers. When the endpoints starts up, if the host is configured for RavenDB persistence and if the configured RavenDB persistence could not be contacted, then warnings will be logged.

**Versions 3.x:** RavenDB is included with the NServiceBus binaries (not ilmerged) and an infrastructure installer ensures that Raven is installed on the local machine when the installers are invoked. The install is only performed if the following is true:

-   RavenDB persistence is configured for the endpoint
-   A custom connection string is not specified by the user
-   The current endpoint has no master node specified
-   Port 8080 is available
-   RavenDB is not already installed
-   The install uses the explicit `/installInfrastructure`

\*When a master node is defined, NServiceBus understands that all data will be stored on the that server instead. This means that the RavenDB server runs on that remote machine and not the local machine where you run the install.

For instructions on how to administrate and operate a RavenDB server, refer to the [documentation for RavenDB](http://ravendb.net/docs/server/administration).

## Upgrading RavenDB

To upgrade an existing RavenDB installation refer to the [RavenDB website article describing the upgrade process](http://ravendb.net/docs/2.0/server/administration/upgrade).

Note it is highly recommended that you backup your Raven database prior to upgrading.

## Which versions of RavenDB are compatible?

NServiceBus V5.0 requires RavenDB v2.5 build 2908 and above.

NOTE: If you already have RavenDB 2.0 installed from a previous NServiceBus V4.0 or prior version and want to run v2.5, you can uninstall the service by finding the Raven.Server.exe executable on your machine and running it from the command line with /uninstall. [Read this document](using-ravendb-uninstalling-v4.md) for full removal instructions.

NServiceBus V4.0 is tested and compatible with RavenDB version 2261 and RavenDB v2.

NServiceBus V3.X is tested and compatible with all RavenDB versions from 616 through 992 and RavenDB v1. We strongly recommend using 992 since it has better support for transaction recovery. Download v992 here: [RavenDB Server - 992](http://hibernatingrhinos.com/builds/ravendb-stable-v1.0/992).

For more information regarding RavenDB compatibility, please refer to [this article](ravendb/version-compatibility.md) and the [RavenDB website](http://ravendb.net/docs/2.0/client-api/backward-compatibility).

## Next steps

You can continue reading about [connecting to RavenDB in NServiceBus](using-ravendb-in-nservicebus-connecting.md) or about [unit of work implementation for RavenDB](unit-of-work-implementation-for-ravendb.md).

