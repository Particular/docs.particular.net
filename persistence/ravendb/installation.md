---
title: Installing RavenDB
summary: How to install RavenDB when using RavenDB persistence for various versions of NServiceBus.
tags:
 - Persistence
reviewed: 2016-10-18
related:
 - nservicebus/operations
redirects:
 - nservicebus/using-ravendb-in-nservicebus-installing
 - nservicebus/ravendb/installation
---

Starting from NServiceBus Version 5.0 RavenDB is no longer the default persistence option in NServiceBus. A user has to explicitly select persistence. For more information, on the various available persistence options, read: [Persistence in NServiceBus](/persistence/).

RavenDB-related code has been moved to a separate package  [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) and it is up to a user to install the RavenDB server from the [official download page](https://ravendb.net/download), following instructions on RavenDB website.

NServiceBus Version 4.x and 3.x used RavenDB for persistence by default. The NServiceBus license permits the use of RavenDB for the storage needs of the endpoint. This only includes NServiceBus-related data, such as sagas and subscriptions. If application-specific data is stored in RavenDB then the purchase of a separate license is required.

To use the default storage, ensure a RavenDB server is running to which the endpoint can connect. To set up and install RavenDB [Download](https://ravendb.net/download).


## NServiceBus auto-installation of RavenDB

**NServiceBus Versions 5.x:** RavenDB is no longer auto-installed.

**NServiceBus Versions 4.x:** The`/installinfrastructure` has been deprecated in Version 4.0 and above on NServiceBus.Host, and NServiceBus now expects RavenDB instance to be pre-installed and accessible to it. Use the [powershell commandlets](/nservicebus/operations/management-using-powershell.md) to install RavenDB on the needed servers. When the endpoints starts up, if the host is configured for RavenDB persistence and if the configured RavenDB persistence could not be contacted, then warnings will be logged.

**NServiceBus Versions 3.x:** RavenDB is included with the NServiceBus binaries (not ilmerged). When using the `/installinfrastructure` switch on the NServiceBus.Host at install time, the infrastructure installer ensures that RavenDB is installed on the local machine. The install is only performed if the following is true:

 * RavenDB persistence is configured for the endpoint
 * A custom connection string is not specified by the user
 * The current endpoint has no master node specified
 * Port 8080 is available
 * RavenDB is not already installed

When a master node is defined, NServiceBus understands that all data will be stored on that designated server  instead. This means that the RavenDB server runs on that remote machine and not the local machine where the install is executed.

See also [Administration of a RavenDB server](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration).


## Upgrading RavenDB

To upgrade an existing RavenDB installation refer to the [RavenDB upgrade process](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration%20upgrade).

It is highly recommended to backup all databases before upgrading.


## RavenDB versions compatible

For a more detailed overview over the compatibility see [RavenDB version compatibility](/persistence/ravendb/version-compatibility.md).

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) greater or equal to Version 3.0.0 requires RavenDB Version 3.0 build 3660 or higher build number for Version 3.0.

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) smaller than Version 3.0.0 requires RavenDB Version 2.5 build 2908 or higher build number for Version 2.5.

NOTE: If RavenDB 2.0 is already installed from a previous NServiceBus Version 4.0 or prior version and want to run Version 2.5, it can be uninstalled by finding the `Raven.Server.exe` executable on the machine and running it from the command line with `/uninstall`. See [Uninstalling RavenDB](uninstalling-v4.md) for full removal instructions. Note that the Version 2.5 download will be unlicensed, so a copy of the license.xml file needs to be taken from the Version 2.0 installation directory to the Version 2.5 installation directory. Contact Particular Software and request a Version 2.5 license file, or use a custom license file.

NServiceBus Version 4.0 is tested and compatible with RavenDB Version 2261 and RavenDB Version 2.

NServiceBus Version 3.X is tested and compatible with all RavenDB versions from Version 616 through version 992 and RavenDB version 1. It is strongly recommend using version 992 since it has better support for transaction recovery. Download version 992 here: [RavenDB Server - 992](https://hibernatingrhinos.com/builds/ravendb-stable-v1.0/992).

For more information regarding RavenDB compatibility, refer to [RavenDB client compatibility](version-compatibility.md) and the [RavenDB website](https://ravendb.net/docs/search/latest/csharp?searchTerm=client-api%20backward-compatibility).
