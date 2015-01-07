---
title: NServiceBus fails during performance counter setup
summary: Sometimes an NServiceBus endpoint hangs or fails during startup while initializing the performance counters due corrupt performance counter libraries
tags: []
---

## Symptoms

Sometimes an NServiceBus endpoint hangs or fails during startup while initializing the performance counters at the following location `NServiceBus.Unicast.Transport.Monitoring.ReceivedPerformanceDiagnostics.SetupCounter`.

## Cause 

This can happen when the performance counter libraries get corrupted.

## Resolution

The performance counters libraries need to be rebuild by doing the following steps:

1. Open an elevated command prompt
2. Execute the following command rebuilds the performance counter libraries:

    `LODCTR /R`  (Mind the uppercase /R)


{{NOTE: Due to the corruption you may have to re-install performance counters of NServiceBus or other products. The following articles help in restoring the NServiceBus performance counters:

 * [Managing NServiceBus using Powershell](managing-nservicebus-using-powershell.md)
 * [Preparing your machine to run NServiceBus](preparing-your-machine-to-run-nservicebus.md)

See the Microsoft support links for more information to restore common Windows performance counters.
}}

## More information

* [Monitoring NServiceBus Endpoints](monitoring-nservicebus-endpoints.md)
* [KB2554336: How to manually rebuild Performance Counters for Windows Server 2008 64bit or Windows Server 2008 R2 systems](http://support.microsoft.com/kb/2554336)
* [KB300956: How to manually rebuild Performance Counter Library values](http://support.microsoft.com/kb/300956) 
* [LODCTR at TechNet](http://technet.microsoft.com/en-us/library/bb490926.aspx)
