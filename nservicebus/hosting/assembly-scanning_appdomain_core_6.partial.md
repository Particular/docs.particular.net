
## AppDomain assemblies

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, the assemblies that are already loaded into the AppDomain, but are not present in the application's base directory, are **not** scanned. The endpoint can also be configured to scan the AppDomain assemblies:

snippet: ScanningApDomainAssemblies
