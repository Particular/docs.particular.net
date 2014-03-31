---
title: How to Develop Custom Checks for ServicePulse
summary: Introduction to ServicePulse custom checks development
tags:
- ServicePulse
- HowTo
- Custom Checks
---

ServicePulse comes with a built-in check that you can install in each endpoint to enable communication with the ServicePulse monitoring service.

You can develop and design custom checks to satisfy all monitoring needs. Bearing in mind that custom checks are intended for monitoring purposes, they can be of two main types:

1. Checks that are executed once and only once at endpoint startup.
1. Checks that are executed periodically.

To develop a custom check, this is all you have to do:

1. In Visual Studio, create a new Class Library project.
1. Add the endpoint plugin to the project using the NuGet console:
	`install-package ServiceControl.Plugin.CustomChecks`
1. Create a custom check:
  * To execute once on endpoint startup, create a class that inherits from `CustomCheck` class (see sample code below).
  * To execute repeatedly, at defined time intervals, create a class that inherits from the `PeriodicCheck` class (see sample code below).
1. Build and deploy the class library DLL in the bin directory of the endpoint that will execute these custom checks.

You can deploy many custom checks per endpoint, and deploy the same custom checks in as many endpoints as required;

##### Sample Custom Check

```C#
using ServiceControl.Plugin.CustomChecks;
using ServiceControl.Plugin.CustomChecks.Messages;
using System;
using System.IO;

namespace CustomCheckSample
{
    class FtpAvailabilityCheck : CustomCheck
    {
        public static bool isAvailable = false;

        public FtpAvailabilityCheck()
            : base("FTP server availability check", "FTP Server") 
        {
            if (!isAvailable) 
                ReportFailed("The FTP Service is down");
        }
    }

    class FtpStorageDirectoryCheck : PeriodicCheck
    {
        public FtpStorageDirectoryCheck() 
            : base("FTP storage directory check", "FTP Server", TimeSpan.FromSeconds(5)){}

        public override CheckResult PerformCheck()
        {
            var dir = @"C:\Checks\FTP";
            if (!Directory.Exists(dir))
            {
                return CheckResult.Failed(string.Format("FTP storage directory '{0}' does not exist", dir));                
            }
            return CheckResult.Pass;
        }
    }
}
```
