## NServiceBus

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as host IDs. MD5 is not FIPS compliant. To run an NServiceBus endpoint under a FIPS policy, override the host identifier using a non-cryptographic hash that is not subject to FIPS enforcement.

A replacement for the MD5-based `DeterministicGuid` utility can be implemented using XxHash128 from the `System.IO.Hashing` package, which targets .NET Standard 2.0 and .NET Framework 4.6.2:

snippet: XxHash128DeterministicGuid

Next, [set the HostId](/nservicebus/hosting/override-hostid.md) as part of the endpoint configuration:

snippet: HostIdFixer