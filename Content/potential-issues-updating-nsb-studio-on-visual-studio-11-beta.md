---
layout:
title: "Potential Issues Updating NSB Studio On Visual Studio 11 Beta"
tags: 
origin: http://www.particular.net/Articles/potential-issues-updating-nsb-studio-on-visual-studio-11-beta
---
**NOTE** : The following notes don't apply if you are downloading NServiceBus Studio from the Visual Studio 2010 Online Gallery using the Extension Manager or if you don't have Visual Studio 11 Beta installed.

This format allows you to install it by simply double clicking on the file, which launches the VSIXInstaller.exe application included as part of Visual Studio.

However, if you have Visual Studio 11 Beta installed on your machine, the VSIX file extension is associated by default with the newer VSIXInstaller included as part of Visual Studio 11 Beta. Unfortunately there are some issues updating VSIX files with the beta version of VSIXInstaller, and if you try to update NServiceBus Studio in the future, you might end up with disabled extensions and conflicts.

To avoid this issue, select one of the following two options.

-   Always uninstall all the NServiceBus extensions before installing a
    new version:

1.  In Visual Studio 2010, in the Extension Manager, uninstall the
    extensions that are part of NServiceBus Studio:

    ![Uninstalling NSB
    Studio](https://particular.blob.core.windows.net/media/Default/images/uninstallingNsbStudio.png)

2.  Uninstall NServiceBusHost, WebEndpoint, WebMVCEndpoint, NServiceBus
    Modeling, and NServiceBus Studio. A Visual Studio restart is
    required.
3.  Install the VSIX file for the new version by double clicking the
    VSIX file.

-   Always use the Visual Studio 2010 VSIXInstaller instead of the beta
    for installing NServiceBusStudio.vsix:

1.  Instead of double clicking on the file, right click it and select
    "Open with".

     In Windows 7 you will see the following window:

    ![Open
    with](https://particular.blob.core.windows.net/media/Default/images/VsixOpenWith.png)

2.  Click "Browse" and find the Visual Studio 2010 VSIXInstaller.exe
    file, typically located in "C:\\Program Files (x86)\\Microsoft
    Visual Studio 10.0\\Common7\\IDE.

In the Windows 8 Consumer Preview:

1.  Select "Choose default program...". In the "How do you want to open
    this type of file (.vsix)?" window, click the "See all..." link at
    the bottom to expand more options.
2.  Scroll down and click "Look for an app on this PC".
3.  Browse for VSIXInstaller.exe in the Visual Studio 10.0 folder,
    typically located in "C:\\Program Files (x86)\\Microsoft Visual
    Studio 10.0\\Common7\\IDE.

![VSIX Installer](https://particular.blob.core.windows.net/media/Default/images/vsixInstaller.png)

**NOTE**: Use this method for future installations of NServiceBus versions.

