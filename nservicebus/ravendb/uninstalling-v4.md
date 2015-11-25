---
title: Uninstall RavenDB v2.0
summary: Article outlines uninstalling RavenDB that were installed by previous versions of the Platform Installer.
tags:
- Persistence
- RavenDB
redirects:
 - nservicebus/using-ravendb-uninstalling-v4
---

As of October 15, 2014 the Platform Installer no longer installs RavenDB version 2.0 in a folder called "NServiceBus.Persistence.v4" under `Program Files` as a prerequisite since NServiceBus version 5 no longer uses it as the default persistence - [see this document](installation.md). If you previously installed this and want to remove it either because you don't need it or you wish to install RavenDB version 2.5 (required version for NServiceBus version 5), then you will need to manually remove it.


## Manual Removal Instructions

1. Run "Raven.Server.exe  /uninstall"  to unregister the service. This doesn't remove the directory, which is by design as the data sub-directory contains any databases you've created.  See [this document](http://ravendb.net/docs/search/latest/csharp?searchTerm=Running-as-a%20service).

2. Remove the registry key "RavenPort" under "HKLM\SOFTWARE\ParticularSoftware\ServiceBus"

3. Remove URLACL for RavenDB Port - it will correspond to the RavenPort listed above, it can also be found in the Raven.Server.config


### URLACL Removal

To list the URACLs for that port, from an **Admin** PowerShell run:

`netsh.exe http show urlacl | select-string :8080`

This may return something like the following:

`Reserved URL            : http://+:8080/`

You can remove it with this command:

`netsh.exe http delete urlacl url=http://+:8080/` 


### Final Folder Cleanup

Remove the folder and sub-folder (assuming you are fine with deleting the RavenDB databases).   

There is one additional cleanup step if you used the PlatformInstaller to install it. The Platform Installer used Chocolatey behind the scenes to install packages. It  keeps a copy of the install in a sub-folder.

The following powershell command will list the directory which you can safely delete.  

`dir ("{0}\lib" -f $env:ChocolateyInstall) -Filter *RavenDB* | Select -ExpandProperty FullName`
