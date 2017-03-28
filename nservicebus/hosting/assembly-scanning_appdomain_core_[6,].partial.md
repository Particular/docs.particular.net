
## AppDomain assemblies

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, NServiceBus does not scan assemblies already loaded into the AppDomain without being present in the applications base directory. The endpoint can be configured to also scan AppDomain assemblies using:

snippet:ScanningApDomainAssemblies
