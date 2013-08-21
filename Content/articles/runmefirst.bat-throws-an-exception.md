<!--
title: "RunMeFirst.bat Throws An Exception"
tags: 
-->

RunMeFirst.bat runs NServiceBusHost.exe with the /installInfrastructure command line.

This, in return, installs the required "infrastructure" components needed to run NServiceBus endpoints.

There are several implications; e.g., the MSMQ RavenDB will be installed, DTC will be checked for proper running, etc.

If you get an exception like this:


Unhandled Exception: System.UnauthorizedAccessException: Cannot create or delete the Performance Category 'nservicebus' because access is denied.


Try to run RunMeFirst.bat as an administrator.

If you get something like this:


Unhandled Exception: System.IO.FileLoadException: Could not load file or assembly 'file:///C:\\NSB\\3.0.4\\binaries\\temp\\lo g4net.dll' or one of its dependencies. Operation is not supported. (Exception from HRESULT: 0x80131515) ---\> System.NotSupportedException: An attempt was made to load an assembly from a network location which would have caused the assembly to be sandboxed in previous versions of the .NET Framework.


Then most likely you should have unblocked the downloaded zip file:

![unblock zip file](https://particular.blob.core.windows.net/media/Default/images/unblock320_crossedout.png "unblock zip file")

