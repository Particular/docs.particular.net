---
title: License file information
summary: Outlines license usage, management, and restrictions
component: core
reviewed: 2021-05-11
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

The license is valid if the `ExpirationDate` or the `UpgradeProtectionExpiration` attribute in the license data is greater than or equal to the release date of the `{major}.{minor}.0` version of the NServiceBus assembly used by the endpoint. To view the release dates for the various versions, see [NServiceBus Packages Versions](/nservicebus/upgrades/all-versions.md).

Note: Only the Major/Minor part is relevant. Eg. if using NServiceBus 6.1.1 it's the release date of 6.1.0 that counts.

## Throughput limitations

No limitations are enforced when either no license is found or a license has expired.

## License management

There are several options available for installing the license file. 

partial: license-management

## Troubleshooting

Diagnose license scanning issues by [enabling Debug logging](/nservicebus/logging/#default-logging-changing-the-defaults-changing-the-logging-level) as all traversed locations and the scan result are logged. 

```txt
2020-04-17 12:11:31.979 DEBUG Looking for license in the following locations:
License not found in S:\docs.particular.net\samples\pubsub\Core_7\Publisher\bin\Debug\netcoreapp2.1\license.xml
License found in C:\Users\XXX\AppData\Local\ParticularSoftware\license.xml
License not found in C:\ProgramData\ParticularSoftware\license.xml
Selected active license from C:\Users\XXX\AppData\Local\ParticularSoftware\license.xml
License Expiration: 2021-01-01
```

NOTE: Identify related log entries by searching/filtering on logger `LicenseManager` or the text `Looking for license in the following locations:`.

### Failed to initialize the license

The license management code requires write permissions to store metadata. If the process credentials don't have write permissions the following fatal event log item can be generated:

```txt
FATAL NServiceBus.Features.LicenseReminder Failed to initialize the license
System.UnauthorizedAccessException: Access to the path 'C:\Windows\system32\config\systemprofile' is denied.
```

Ensure that the process has write permissions at the specified location. If necessary, either modify the location by using another suitable [license management technique](/nservicebus/licensing/#license-management), change permissions, or use another process that has appropriate permissions.
