---
title: Installation 
summary: 'Installing ServiceMatrix'
tags:
- ServiceMatrix
- Visual Studio
- Troubleshooting
---

This article reviews how to download and install ServiceMatrix for use in Visual Studio.

## Prerequisites

ServiceMatrix is a Visual Studio extension. Version 2.1 and beyond require that you have Visual Studio 2012 or 2013 on your system prior to installation. You will need the Professional or above edition of Visual Studio (Express editions not supported).
 
If you run both Visual Studio 2013 and 2012, you may install the proper version in each version on the same system.

## Installing ServiceMatrix

To install ServiceMatrix the VSIXInstaller.exe is required which is shipped with commercial versions of Visual Studio ( i.e Visual Studio Professional or better). The Express editions of Visual Studio are not supported.

### From Visual Studio Gallery

From your Visual Studio IDE, go to Tools -> Extensions and Updates and Search for `ServiceMatrix` online. You can directly download and install from the `Tools and Extensions` dialog.

### Download the VSIX and install

You can download the VSIX from here: [ServiceMatrix Releases](https://github.com/Particular/ServiceMatrix/releases/latest). 

The file name for the Visual Studio 2013 version is `Particular.ServiceMatrix.12.0.vsix`.
The file name for the Visual Studio 2012 version is `Particular.ServiceMatrix.11.0.vsix`.

The VSIX file can either be double-clicked on to install it or run from the command line. The following example shows the installation using Visual Studio 2013.

```bat
"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDEVsixinstaller.exe" Particular.ServiceMatrix.12.0.vsix 
```

WARN: ServiceMatrix requires an Internet connection to download packages from the NuGet public feed. So whilst ServiceMatrix can be installed offline without a NuGet feed it will not be able to create and manage projects properly.



### Installing the ServiceMatrix Visual Studio extension (VSIX)

1. Download the latest version of ServiceMatrix for your version of Visual Studio from [Particular.net](http://particular.net/downloads).
2. Save the .vsix file in your favorite location.
3. Locate and run the ServiceMatrix installer VSIX file you downloaded. Since ServiceMatrix is an extension to Visual Studio, it's a good idea to close Visual Studio. If you don't you will be prompted later.
4. If you already have ServiceMatrix installed and are upgrading from a Beta version, you will get a [warning](images/servicematrix-installer-existingversion.png "Previous Version Warning") that you must uninstall the previous version. Uninstall using the normal procedure through `Control Panel\Programs\Programs and Features` in Windows.

## The Visual Studio Extension

When you start Visual Studio, you can review the ServiceMatrix extension. To do so, select 'Tools' [from the menu](images/servicematrix-vstudio-toolsmenu.png "Extensions Menu") and select 'Extensions and Updates'. Notice the extension listed as shown below.

![Visual Studio Extensions](images/servicematrix-vstudio-extensions.png)

## Troubleshooting

If you have issues installing or uninstalling ServiceMatrix please see the [troubleshooting](troubleshooting-servicematrix-2.0.md "Troubleshooting ServiceMatrix") article.

## Next steps

### Getting started examples

At the completion of the installation your default browser will open to the first of a series of ServiceMatrix [Getting Started](getting-started-with-servicematrix-2.0.md "Getting Started With ServiceMatrix") documents. 

### Licensing
ServiceMatrix installs with a trial license. Read more about how the product is licensed [here](licensing-servicematrix-v2.0.md "Licensing NServiceBus").
