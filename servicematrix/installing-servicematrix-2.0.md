---
title: Installation  
summary: 'Installing ServiceMatrix'
tags:
- ServiceMatrix
- Visual Studio
- Troubleshooting
---

ServiceMatrix enables developers to efficiently generate a fully functional distributed NServiceBus solution using an intuitive visual interface.  Using the drawing canvas you can create a solution composed of endpoints, services, components and messages.   The resulting Visual Studio solution contains generated code that adheres to best practices and includes developer-friendly extension points for integration with your custom code.   

If you're ready to learn more, this article reviews how to download and install ServiceMatrix for use in Visual Studio.

1.  [Prerequisites](#prerequisites)
2.  [Installing ServiceMatrix](#installing-servicematrix)
3.  [The Visual Studio extension](#visual-studio-extension)
3.  [Troubleshooting](#troubleshooting)
4.  [Next steps](#next-steps)

## Prerequisites

ServiceMatrix is a Visual Studio extension.  Version 2.1 and beyond require that you have Visual Studio 2012 or 2013 on your system prior to installation. You will need the Professional or above edition of Visual Studio (Express editions not supported).
  
If you are using Visual Studio 2010, ServiceMatrix 1.0 will be detected and installed. Version 1.0 builds solutions through a tree view in the solution builder interface. New features like the drawing canvas are only available in V2.X.  Version 1.0 will no longer be updated with new features.

If you run both Visual Studio 2013 and 2012, you may install the proper version in each version on the same system.  If you have Visual Studio 2010 installed along with Visual Studion 2012 or 2013, you must choose to install ServiceMatrix for one platform or the other.

## Installing ServiceMatrix

You can install ServiceMatrix by using the [Platform Installer](/platform/installer) or by installing the Visual Studio extension (VSIX) from the [Particular Software website downloads page](http://particular.net/downloads)

### Installing ServiceMatrix from the Platform Installer

Please refer to the [Platform Installer](/platform/installer) for detailed information.

### Installing the ServiceMatrix Visual Studio extension (VSIX)

1. Download the latest version of ServiceMatrix for your version of Visual Studio from [Particular.net](http://particular.net/downloads). 
2. Save the .vsix file in your favorite location.
3. Locate and run the ServiceMatrix installer VSIX file you downloaded.  Since ServiceMatrix is an extension to Visual Studio, it's a good idea to close Visual Studio. If you don't you will be prompted later. 
4. If you already have ServiceMatrix installed and are upgrading from a Beta version, you will get a [warning](images/servicematrix-installer-existingversion.png "Previous Version Warning") that you must uninstall the previous version. Uninstall using the normal procedure through `Control Panel\Programs\Programs and Features` in Windows.

## The Visual Studio Extension

When you start Visual Studio, you can review the ServiceMatrix extension.  To do so, select 'Tools' [from the menu](images/servicematrix-vstudio-toolsmenu.png "Extensions Menu") and select 'Extensions and Updates'.  Notice the extension listed as shown below.

![Visual Studio Extensions](images/servicematrix-vstudio-extensions.png)

## Troubleshooting

If you have issues installing or uninstalling ServiceMatrix please see the [troubleshooting](troubleshooting-servicematrix-2.0.md "Troubleshooting ServiceMatrix") article. 

## Next steps

### Getting started examples

At the completion of the installation your default browser will open to the first of a series of ServiceMatrix [Getting Started](getting-started-with-servicematrix-2.0.md "Getting Started With ServiceMatrix") documents.  

### Licensing
ServiceMatrix installs with a trial license. Read more about how the product is licensed [here](licensing-servicematrix-v2.0.md "Licensing NServiceBus").
