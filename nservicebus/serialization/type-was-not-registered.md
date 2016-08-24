---
title: Type Not Registered in the Serializer
summary: Type may not be registered. This may happen after an upgrade. Is usually due to locked files.
reviewed: 2016-03-17
redirects:
 - nservicebus/type-was-not-registered-in-the-serializer
---

If this exception is encountered when using NServiceBus in an ASP.NET application after upgrading the code of the app, it is usually due to files that are locked.


## Exception details

> Server Error in '/' Application. Type XYZ was not registered in the serializer. Check that it appears in the list of configured assemblies/types to scan. Description: An unhandled exception occurred during the execution of the current web request. Review the stack trace for more information about the error and where it originated in the code.


## Solution

 1. Stop the [w3svc process](https://technet.microsoft.com/en-us/library/cc734944.aspx) of the website being upgraded.
 1. Delete temporary ASP.NET files, usually found in the directory `c:\Windows\Microsoft.NET\Framework(64)\{version}\`.
 1. Run the application again. The exception should no longer happen.