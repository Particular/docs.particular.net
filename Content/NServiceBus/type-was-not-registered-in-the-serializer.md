---
title: Type Not Registered in the Serializer
summary: Type may not be registered. This may happen after you upgrade your app code. Is usually due to locked files.
originalUrl: http://www.particular.net/articles/type-was-not-registered-in-the-serializer
tags: []
createdDate: 2013-05-22T08:23:53Z
modifiedDate: 2013-11-25T13:37:58Z
authors: []
reviewers: []
contributors: []
---

If you run into this exception when using NServiceBus in an ASP.NET application after upgrading the code of your app, it is usually due to files that are locked.

Exception details
-----------------


Server Error in '/' Application. Type XYZ was not registered in the serializer. Check that it appears in the list of configured assemblies/types to scan. Description: An unhandled exception occurred during the execution of the current web request. Review the stack trace for more information about the error and where it originated in the code.


Solution
--------

1.  Stop the w3svc process of the website whose code you are upgrading.
2.  Delete temporary ASP.NET files, usually found in the folder
    c:\\Windows\\Microsoft.NET\\Framework(64)\\{version}\\ .
3.  Run your application again. The exception should no longer happen.


