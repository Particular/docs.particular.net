---
title: Licensing
summary: Outlines license usage, management, and restrictions
component: core
reviewed: 2017-08-15
redirects:
 - nservicebus/licensing-limitations
 - nservicebus/licensing/licensing-limitations
 - nservicebus/licensing/license-management
 - nservicebus/license-management
related:
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/support-policy
 - servicecontrol/license
 - serviceinsight/license
---


## License details

See the [Licensing page](https://particular.net/licensing) for license specifics.


## License validity

partial: validity


## Throughput limitations

partial: limitations


## License management

There are several options available for installing the license file. 

partial: license-management


## Troubleshooting

### Failed to initialize the license

The license management code requires write permissions to store metadata. If the process credentials don't have write permissions the following fatal event log item can be generated:

```
FATAL NServiceBus.Features.LicenseReminder Failed to initialize the license
System.UnauthorizedAccessException: Access to the path 'C:\Windows\system32\config\systemprofile' is denied.
```

Ensure that the process has write permissions at the specified location. If necessary, either modify the location, change permissions, or use another process that has appropriate permissions.
