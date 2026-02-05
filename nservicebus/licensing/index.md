---
title: License file information
summary: Learn how NServiceBus licensing works, including configuration, license types, usage reporting, and expiration
component: core
reviewed: 2024-05-07
redirects:
 - nservicebus/licensing-limitations
 - nservicebus/licensing/licensing-limitations
 - nservicebus/licensing/license-management
 - nservicebus/license-management
related:
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/support-policy
 - servicecontrol/license
---

## License details

See the [Licensing page](https://particular.net/licensing) for license specifics.

## License validity

The license is valid if the current UTC date has not passed the `ExpirationDate` attribute date value.

### Upgrade protection

Some licenses have a date value in the `UpgradeProtectionExpiration` attribute. In this case the license is still valid if the `UpgradeProtectionExpiration` attribute date value is greater than or equal to the release date of the `{major}.{minor}.0` version of the NServiceBus assembly used by the endpoint. To view the release dates for the various versions, see [NServiceBus Packages Versions](/nservicebus/upgrades/all-versions.md).

> [!NOTE]
> Only the Major/Minor part is relevant. Eg. if using NServiceBus 6.1.1 it's the release date of 6.1.0 that counts.

## Throughput limitations

No technical limitations are enforced at runtime when either no license is found, invalid or corrupt license data is provided, or a license has expired.

## License management

There are several options available for installing the license file.

partial: license-management

Sometimes the license must be embedded in a single line of text, for example, in a command line when deploying an endpoint with Docker. For these scenarios, the license can be minified, removing all spaces and line-breaks, by adding `minify=true` to the query string of the URL used to download a license file from the Particular Software website.

## Behavior

The license is only read once at startup. When the license expires the endpoint logs will contain a message indicating the license is expired. To resolve this the endpoint must be restarted so that it can read the updated license at startup.

## Troubleshooting

Diagnose license scanning issues by [enabling Debug logging](/nservicebus/logging/#default-logging-changing-the-defaults-changing-the-logging-level) as all traversed locations and the scan result are logged.

```txt
2020-04-17 12:11:31.979 DEBUG Looking for license in the following locations:
License not found in S:\docs.particular.net\samples\pubsub\Core_7\Publisher\bin\Debug\net5.0\license.xml
License found in C:\Users\XXX\AppData\Local\ParticularSoftware\license.xml
License not found in C:\ProgramData\ParticularSoftware\license.xml
Selected active license from C:\Users\XXX\AppData\Local\ParticularSoftware\license.xml
License Expiration: 2021-01-01
```

> [!NOTE]
> Identify related log entries by searching/filtering on logger `LicenseManager` or the text `Looking for license in the following locations:`.

### Failed to initialize the license

The license management code requires write permissions to store metadata. If the process credentials don't have write permissions the following fatal event log item can be generated:

```txt
FATAL NServiceBus.Features.LicenseReminder Failed to initialize the license
System.UnauthorizedAccessException: Access to the path 'C:\Windows\system32\config\systemprofile' is denied.
```

Ensure that the process has write permissions at the specified location. If necessary, either modify the location by using another suitable [license management technique](/nservicebus/licensing/#license-management), change permissions, or use another process that has appropriate permissions.
