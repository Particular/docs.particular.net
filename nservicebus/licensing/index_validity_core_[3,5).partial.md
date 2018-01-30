### Version 3 to 4.5 

The license is valid if the `LicenseVersion` attribute in the license data is greater than or equal to the `{major}.{minor}` part of the fileversion for the NServiceBus assembly used by the endpoint.


### Version 4.5 to 5 

The license is valid if the `ExpirationDate` or the `UpgradeProtectionExpiration` attribute in the license data is greater than or equal to the release date of the `{major}.{minor}.0` version of the NServiceBus assembly used by the endpoint. To view the release dates for the various versions, see [NServiceBus Packages Versions](/nservicebus/upgrades/all-versions.md).

Note: Only the Major/Minor part is relevant. Eg. if using NServiceBus 6.1.1 it's the release date of 6.1.0 that counts.
