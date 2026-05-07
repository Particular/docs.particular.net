## NServiceBus

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as host IDs. MD5 is not FIPS compliant. Using a FIPS-approved hashing algorithm like SHA256 to generate a HostId will allow an NServiceBus endpoint to run under a FIPS policy.

> [!NOTE]
> Host IDs are non-cryptographic identifiers used for endpoint monitoring and correlation. Using a cryptographic hash like SHA256 for this purpose is unnecessarily expensive and not required for security. Starting in version 10.2, NServiceBus uses XxHash128, a non-cryptographic hash that is not subject to FIPS policy enforcement, avoiding the need for cryptographic workarounds entirely.

First, a replacement for the MD5 based DeterministicGuid utility within the NServiceBus framework will have to be provided:

snippet: SHA256DeterministicGuid

Next, [set the HostId](/nservicebus/hosting/override-hostid.md) as part of the endpoint configuration:

snippet: HostIdFixer