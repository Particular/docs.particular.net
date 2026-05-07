## NServiceBus

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as HostIds. MD5 is not FIPS compliant. Using a FIPS compliant hashing algorithm method to generate a HostId will allow an NServiceBus endpoint to run under a FIPS policy.

First, a replacement for the MD5 based DeterministicGuid utility within the NServiceBus framework will have to be provided:

snippet: SHA1DeterministicGuid

Next, [set the HostId](/nservicebus/hosting/override-hostid.md) as part of the endpoint configuration:

snippet: HostIdFixer