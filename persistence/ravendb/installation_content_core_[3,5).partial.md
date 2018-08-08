Starting from NServiceBus Version 5.0, RavenDB is no longer the default persistence option in NServiceBus; a persistence must be explicitly selected. For more information, on the available persistence options, read: [Persistence in NServiceBus](/persistence/).

RavenDB-related code has been moved to a separate package  [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) and a RavenDB server must be installed from the [official download page](https://ravendb.net/download), following instructions on the RavenDB website.

NServiceBus version 4.x and 3.x use RavenDB for persistence by default.

To use the default storage, ensure a RavenDB server is running to which the endpoint can connect.


## NServiceBus auto-installation of RavenDB

**NServiceBus version 5.x:** RavenDB is no longer auto-installed.

**NServiceBus version 4.x:** The`/installinfrastructure` has been deprecated in version 4.0 and above on NServiceBus.Host, and NServiceBus now expects a RavenDB instance to be pre-installed and accessible. Use the [powershell commandlets](/nservicebus/operations/management-using-powershell.md) to install RavenDB on the necessary servers. When the endpoints starts, if the host is configured for RavenDB persistence and if the configured RavenDB persistence can not be contacted, then warnings will be logged.

**NServiceBus version 3.x:** RavenDB is included with the NServiceBus binaries (not ilmerged). When using the `/installinfrastructure` switch on the NServiceBus.Host at install time, the infrastructure installer ensures that RavenDB is installed on the local machine. The install is only performed if the following is true:

 * RavenDB persistence is configured for the endpoint
 * A custom connection string is not specified by the user
 * The current endpoint has no master node specified
 * Port 8080 is available
 * RavenDB is not already installed

When a master node is defined, NServiceBus understands that all data will be stored on that designated server instead. This means that the RavenDB server runs on that remote machine and not the local machine where the install is executed.

See also: [Administration of a RavenDB server](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration).


## Upgrading RavenDB

To upgrade an existing RavenDB installation, refer to the [RavenDB upgrade process](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration%20upgrade).

It is highly recommended to backup all databases before upgrading.


## RavenDB version compatibility

For a more detailed overview of the compatibility see [RavenDB version compatibility](/persistence/ravendb/version-compatibility.md).

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) greater or equal to version 3.0.0 requires RavenDB version 3.0 build 3660 or higher.

[NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB) less than version 3.0.0 requires RavenDB version 2.5 build 2908 or higher.

NOTE: If RavenDB 2.0 is already installed from a previous NServiceBus version 4.0 or prior version, it can be uninstalled by finding the `Raven.Server.exe` executable on the machine and running it from the command line with `/uninstall`. See [Uninstalling RavenDB](uninstalling-v4.md) for full removal instructions. Note that the version 2.5 download will be unlicensed, so a copy of the license.xml file must be taken from the version 2.0 installation directory to the version 2.5 installation directory.

NServiceBus version 4.0 is tested and compatible with RavenDB version 2261 and RavenDB version 2.

NServiceBus version 3.X is tested and compatible with all RavenDB versions from version 616 through version 992 and RavenDB version 1. It is strongly recommend to use version 992 since it has better support for transaction recovery. 

For more information regarding RavenDB compatibility, refer to [RavenDB client compatibility](version-compatibility.md) and the [RavenDB website](https://ravendb.net/docs/search/latest/csharp?searchTerm=client-api%20backward-compatibility).
