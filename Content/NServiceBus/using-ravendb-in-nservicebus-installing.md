---
title: Installing RavenDB for NServiceBus
summary: Starting from version 3.0, NServiceBus uses RavenDB for persistence by default. The NServiceBus license permits the use of RavenDB for the storage needs of your endpoint. This only includes NServiceBus-related data such as sagas and subscriptions. If you store application-specific data in RavenDB you need to purchase a separate license.
originalUrl: http://www.particular.net/articles/using-ravendb-in-nservicebus-installing
tags:
- Persistence
- RavenDB
---

Starting from version 3.0, NServiceBus uses RavenDB for persistence by default. The NServiceBus license permits the use of RavenDB for the storage needs of your endpoint. This only includes NServiceBus-related data such as sagas and subscriptions. If you store application-specific data in RavenDB you need to purchase a separate license.

To use the default storage, ensure you have a RavenDB server running to which your endpoint can connect. There are two ways to set it up:

-   [Download](http://ravendb.net/download) and install RavenDB yourself
    (if you already use Raven or if you have other specific requirements
    for the installation)
-   Let NServiceBus do it for you

NServiceBus auto-installation of RavenDB
----------------------------------------

**Version 3.0:** RavenDB is included with the NServiceBus binaries (not ilmerged) and an infrastructure installer ensures that Raven is installed on the local machine when the installers are invoked. The install is only performed if the following is true:

-   RavenDB persistence is configured for the endpoint
-   A custom connection string is not specified by the user
-   The current endpoint has no master node specified\*
-   Port 8080 is available
-   RavenDB is not already installed
-   The install uses the explicit /installInfrastructure

\*When a master node is defined, NServiceBus understands that all data will be stored on the that server instead. This means that the RavenDB server runs on that remote machine and not the local machine where you run the install.

**Version 4.0:**The /installinfrastructure has been deprecated in 4.0 on NServiceBus.Host. Use the [powershell commandlets](managing-nservicebus-using-powershell.md) to install RavenDB on the needed servers. When the endpoints starts up, if the host is configured for RavenDB persistence and if the configured RavenDB persistence could not be contacted, then warnings will be logged.

For instructions on how to administrate and operate a RavenDB server, refer to the [documentation for RavenDB](http://ravendb.net/docs/server/administration).

Upgrading RavenDB
-----------------

To upgrade an existing RavenDB installation refer to the [RavenDB website article describing the upgrade process](http://ravendb.net/docs/2.0/server/administration/upgrade).

Note it is highly recommended that you backup your Raven database prior to upgrading.

Which versions of RavenDB are compatible?
-----------------------------------------

NServiceBus V3.X is tested and compatible with all RavenDB versions from
616 through 992 and RavenDB v2. We strongly recommend using 992 since it has better support for transaction recovery. Download v992 here:
[RavenDB Server -
992](http://hibernatingrhinos.com/builds/ravendb-stable-v1.0/992).

NServicedBus V4.0 is tested and compatible with RavenDB version 2261 and RavenDB v2.

For more information regarding RavenDB compatibility, please refer to the [RavenDB website](http://ravendb.net/docs/2.0/client-api/backward-compatibility).

Next steps
----------

You can continue reading about [connecting to RavenDB in NService bus](using-ravendb-in-nservicebus-connecting.md) or about [unit of work implementation for RavenDB](unit-of-work-implementation-for-ravendb.md).

