---
title: Building NServiceBus from Source Files
summary: Build NServiceBus from its source files, noting entry points, Build.ps1, and build.bat.
originalUrl: http://www.particular.net/articles/building-nservicebus-from-source-files
tags: []
---

Getting the source
==================

Cloning the git repository
--------------------------

If you want all the source for all versions of NServiceBus you should
"Clone the repository". The recommended approaches are

-   Using [GitHub for Windows](http://windows.github.com/)
-   Using the [Git command line](http://git-scm.com/book/en/Git-Basics-Getting-a-Git-Repository)

Downloading a zip
-----------------

GitHub supports downloading a zip file of the source. The convention is
https://github.com/NServiceBus/NServiceBus/archive/{branchOrTagName}.zip. So for example

-   The current develop branch [https://github.com/NServiceBus/NServiceBus/archive/develop.zip](https://github.com/NServiceBus/NServiceBus/archive/develop.zip)
-   The current master branch is
    [https://github.com/NServiceBus/NServiceBus/archive/master.zip](https://github.com/NServiceBus/NServiceBus/archive/master.zip)
-   The 3.3.8 tag is
    [https://github.com/NServiceBus/NServiceBus/archive/3.3.8.zip](https://github.com/NServiceBus/NServiceBus/archive/3.3.8.zip)

Building NServiceBus 4
======================

See "Building" in the `Readme.md` file at the root of the repository

Building NServiceBus 3
======================

To build NServiceBus 3 from its source files, download the source files from
[github](https://github.com/NServiceBus/NServiceBus/zipball/develop) to get the latest unstable build, or retrieve the latest stable build source code from the [downloads page](http://nservicebus.com/downloads.aspx).

Build NServiceBus options
-------------------------

You can build NServiceBus by either typing .\\build.ps1 in a PowerShell command line or by executing build.bat from the command line. Both files executes default.ps1, which is a PowerShell script file that builds NServiceBus. Default.ps1 is based on the
[psake](http://github.com/psake/psake) build automation tool.

Logical entry points
--------------------

Default.ps1 has five major logical entry points or main tasks:

 * Default: No need to call explicitly. It calls ReleaseNServiceBus task
 * ReleaseNServiceBus:
    1.  Compiles all the source code in order.
    2.  Runs the unit tests.
    3.  Prepares the binaries and core-only binaries.
    4.  Creates and publishes the packages to the NuGet gallery (access key required).
    5.  Archives release artifacts in zip.
 * JustPrepareBinaries shortens build time:
    1.  Compiles all the source code.
    2.  Prepares the binaries.
 * PrepareBinaries:
    1.  Compiles all the source code in order.
    2.  Runs the unit tests.
    3.  Prepares the binaries and core-only binaries.
 * PrepareReleaseWithoutSamples:
    1.  Compiles all the source code except samples.
    2.  Runs the unit tests.
    3.  Prepares the binaries and core-only binaries.

Entry points for development
----------------------------

Entry points ease the build activity for development:

-   CompileMain - Builds NServiceBus.dll and keeps the output in `\binaries`.
-   TestMain - Builds NServiceBus.dll, keeps the output in `\binaries`.
-   CompileCore - Builds NServiceBus.Core.dll and keeps the output in `\binaries`.
-   TestCore - Builds NServiceBus.Core.dll and keeps the output in `\binaries`.
-   CompileContainers - Builds the container dlls for autofac, castle, ninject, spring, structuremap, and MS unity and keeps the output in respective folders in `\binaries\containers`.
-   TestContainers - Builds the container dlls for autofac, castle, ninject, spring, structuremap, and MS unity and keeps the output in respective folders in binaries\\containers and unit tests.
-   CompileWebServicesIntegration - Builds `NServiceBus.Integration.WebServices.dll` and keeps the output in
    `\binaries`.
-   CompileNHibernate - Builds `NServiceBus.NHibernate.dll` and keeps the output in `\binaries`.
-   TestNHibernate - Builds `NServiceBus.NHibernate.dll`, keeps the output in `\binaries` and unit tests.
-   CompileHosts - Builds `NServiceBus.Host.exe` and keeps the output in `\binaries`.
-   CompileHosts32 - Builds `NServiceBus.Host32.exe` and keeps the output in `\binaries`.
-   CompileAzure - Builds `NServiceBus.Azure.dll` and keeps the output in `\binaries`.
-   TestAzure - Builds `NServiceBus.Azure.dll`, keeps the output in `\binaries`.
-   CompileAzureHosts - Builds `NServiceBus.Hosting.Azure.dll` and `NServiceBus.Hosting.Azure.HostProcess.exe` and keeps the output in `\binaries`.
-   CompileTools - Builds the tools like `XsdGenerator.exe`, `runner.exe` and `ReturnToSourceQueue.exe` and unit tests.
-   CompileSamples - Compiles all the sample projects.
-   CompileSamplesFull - Compiles all the sample projects after compiling the source.

Build.ps1 script
----------------

Build.ps1 is a PowerShell script that is used to execute the default.ps1 NServiceBus build script. Following are examples for executing the Build.ps1 script:

-   Array of strings that takes the names of the tasks to be executed.

An example of using it: you are working on a sample, making modifications in one of the NServiceBus main libraries. To see the effect of the change (to build the NServiceBus main libraries), execute this:

    NServiceBus> .\build.ps1 @("CompileMain", "CompileSamples")

OR

    NServiceBus> .\build.ps1 CompileMain

    \$properties = @{} 

enables passing the properties value when invoking default.ps1, for example:

    NServiceBus> .\build.ps1 @("InstallDependentPackages","CompileSamples") @{DownloadDependentPackages=$true}

    \$desc, pass this parameter to see a description of the task, for
    example:

    NServiceBus> .\build.ps1 -desc@("InstallDependentPackages","CompileSamples") @{DownloadDependentPackages=$true}

The examples assume that you are executing from the NServiceBus root folder at the PowerShell command prompt or the VisualStudio Package Manager console.

Build.bat
---------

Build.bat is an easy way to execute NServiceBus building processes. Examples:

    > build.bat "@(\"CompileMain\",\"CompileSamples\")"
    > build.bat "@(\"InstallDependentPackages\",\"CompileSamples\") @{DownloadDependentPackages=$true}"