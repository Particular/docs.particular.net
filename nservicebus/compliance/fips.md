---
title: FIPS Compliance
summary: Describes NServiceBus compliance with FIPS
reviewed: 2019-02-28
---

The [Federal Information Processing Standards](https://en.wikipedia.org/wiki/Federal_Information_Processing_Standards) or FIPS are standards developed by the United States government for computer systems that sets requirements for, among other things, crytography.

The Particular Software Platform is not FIPS compliant, but can run in a FIPS compliant using the following methods:

## NServiceBus HostId

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as HostIds. MD5 is not FIPS compliant. Using a FIPS compliant hashing algorithm method to generate a HostId will allow an NServiceBus endpoint to run under a FIPS policy.

First a replacement for the MD5 based DeterministicGuid utility within the NServiceBus framework will have to be provided:

snippet: SHA1DeterministicGuid

Next [set the HostId](https://docs.particular.net/nservicebus/hosting/override-hostid) as part of the endpoint configuration:

snippet: HostIdFixer

## Disable enforcement of FIPS

FIPS enforcement can be disabled by setting the runtime setting `enforceFIPSPolicy` to `false` in the applications app.config or web.config. See the [MSDN article on how to change this setting](https://msdn.microsoft.com/en-us/library/hh202806(v=vs.110).aspx).
