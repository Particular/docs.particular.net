
## AppDomain assemblies

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, already loaded into the AppDomain, but not present in the applications base directory, are **not** scanned. The endpoint can be configured to also scan AppDomain assemblies using:

snippet: ScanningApDomainAssemblies
