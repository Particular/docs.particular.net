---
title: 'RavenDB Persistence: Uninstall RavenDB v2.0'
summary: Article outlines uninstalling RavenDB that were installed by previous versions of the Platform Installer.
tags:
- Persistence
- RavenDB
redirects:
 - nservicebus/using-ravendb-uninstalling-v4
---

As of 2014-10-15, the Platform Installer no longer installs RavenDB Version 2.0 in a folder called "NServiceBus.Persistence.v4" under `Program Files` as a prerequisite since NServiceBus Version 5 no longer uses it as the default persistence - [see RavenDB installation](installation.md). If previously installed this and want to remove it will be necessary to be done manually.


## Manual Removal Instructions

 1. Run "Raven.Server.exe /uninstall" to unregister the service. This doesn't remove the directory, which is by design as the data sub-directory contains any created databases. See [RunningRavendDB as a service](http://ravendb.net/docs/search/latest/csharp?searchTerm=Running-as-a%20service).
 1. Remove the registry key "RavenPort" under "HKLM\SOFTWARE\ParticularSoftware\ServiceBus".
 1. Remove URLACL for RavenDB Port - it will correspond to the RavenPort listed above, it can also be found in the `Raven.Server.config`.


### URLACL Removal

To list the URACLs for that port, from an **Admin** PowerShell run:

```dos
netsh.exe http show urlacl | select-string :8080
```

This may return something like the following:

```dos
Reserved URL: http://+:8080/
```

Remove it with this command:

```dos
netsh.exe http delete urlacl url=http://+:8080/
```


### Final Folder Cleanup

Remove the folder and sub-folder (assuming the of RavenDB databases are are no longer needed).

There is one additional cleanup step if the PlatformInstaller was used as the method of install. The Platform Installer used Chocolatey behind the scenes to install packages. It  keeps a copy of the install in a sub-folder.

The following PowerShell command will list the directory which can be safely deleted.

```dos
dir ("{0}\lib" -f $env:ChocolateyInstall) -Filter *RavenDB* | Select -ExpandProperty FullName
```