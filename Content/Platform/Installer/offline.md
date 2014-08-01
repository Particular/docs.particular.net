---
title: Installing The Platform Componennts Manually - Offline
summary: 'Guidance on how to install the platform components offline'
tags: [Platform, Installation, Offline]
---

The PI handles installing pre-requisites for NSB and the platform product installs (SI, SM, SP & SC) - it doesn't handle any NSB nugets
The PI is hard coded to make use of chocolatey & chocolatey packages,  and those packages contain URLs for the sources for the binaries.
For the pre-requisites they could use PowerShell Scripts contained in the chocolatey packages to configure up either dev or production machine. ( See below) 
For SI, SP, SC & SM  they can download the standalone installers from here: http://www.particular.net/downloads
There is a problem with running SM disconnected from the Internet - it prompt to packages from a feed
The way around this would be to setup a private nuget server (http://docs.nuget.org/docs/creating-packages/hosting-your-own-nuget-feeds)
and then host the NSB packages on it

As mention above - here's the manual steps for the prerequisites:

## MSMQ 
Copy this file and execute -  https://github.com/Particular/Packages.Msmq/blob/master/src/tools/setup.ps1

## MSDTC  -
 Copy these two files:

https://github.com/Particular/Packages.DTC/blob/master/src/tools/setup.ps1
https://github.com/Particular/Packages.DTC/blob/master/src/tools/RegHelper.cs

Execute the Setup.PS1

## PerfMon Counters
Copy this file and execute https://github.com/Particular/Packages.PerfCounters/blob/master/src/tools/setup.ps1

## RavenDB 
The PI installs Raven 2.0 - if the customer wants to use Raven 2.5 then this step isn't appropriate

Unfortunately the PS script for this Raven 2 choc package is not suitable for offline setup  ( The script I'm referring to is here    https://github.com/Particular/Packages.RavenDB/blob/master/src/tools/setup.ps1  )
The manual steps to achieve the same thing are:

Decide what port number raven will listen on - we default to 8080
Download   https://s3.amazonaws.com/particular.downloads/PlatformInstaller.Raven/Raven.2375-NServiceBus.zip   (This URL is case sensitive) 
The zip contains the Raven binaries and the raven license file
Unzip the contents to "C:\Program Files\NServiceBus.Persistence.v4"
Edit the Raven.Server.exe.config to change the port 
     e.g  <add key="Raven/Port" value="*"/>  becomes  <add key="Raven/Port" value="8080"/>  

From an admin command prompt :
    cd  "C:\Program Files\NServiceBus.Persistence.v4"
    Raven.Server.exe  --install --service-name="RavenDB"

Add an appropriate URLACL (again assuming port number is 8080 and replace domain and user name with appropriate values)

        
e.g.

     Netsh.exe http add urlacl url=http://+:8080/ user=<domain>\<user>
