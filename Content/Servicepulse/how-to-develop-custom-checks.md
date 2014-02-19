#### HOWTO: Develop Custom Checks for ServicePulse

1. In Visual Studio, create a new Class Library project 
* Add the Endpoint Plugin to the project using the NuGet console: 
      * ```install-package ServiceControl.Plugin.CustomChecks -pre```
* To create a custom check that executes once, on endpoint startup, create a class that inherits from ```CustomCheck``` class (see sample code below)
* To create a custom check that executes repeatedly, in defined time intervals, create a class that inherits from ```PeriodicCheck``` class (see sample code below)
* Build and deploy the class library dll in the Bin directory of the endpoint you wish to execute these custom checks
   * You can deploy many custom checks per endpoint, and deploy the same custom checks in as many endpoints as required 

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