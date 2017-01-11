### Version 3 to 4.5 

The license is valid if the `LicenseVersion` attribute in the license data is greater than or equal to the `{major}.{minor}` part of the fileversion for the NServiceBus assembly used by the endpoint.

### Version 4.5 to 5 

The license is valid if the `ExpirationDate` or the `UpgradeProtectionExpiration` attribute in the license data is greater than or equal to the release date of the `{major}.{minor}.0` version of the NServiceBus assembly used by the endpoint. The release dates for the various versions can be found on [NuGet](https://www.nuget.org/packages/nservicebus).

Note: Only the Major/Minor part is relevant. Eg. if using NServiceBus 6.1.1 it's the release date if 6.1.0 that counts.