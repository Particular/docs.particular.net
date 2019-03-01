---
title: FIPS Compliance
summary: Describes NServiceBus compliance with FIPS
reviewed: 2019-02-28
---

The [Federal Information Processing Standards](https://en.wikipedia.org/wiki/Federal_Information_Processing_Standards) or FIPS are standards developed by the United States government for computer systems that sets requirements for, among other things, crytography.

Microsoft [does not recommend enabling FIPS](https://blogs.technet.microsoft.com/secguide/2014/04/07/why-were-not-recommending-fips-mode-anymore/) unless it is required by government regulations.

The Particular Software Platform is not FIPS compatible, but your endpoints can run with FIPS enabled using the following method:

## NServiceBus HostId

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as HostIds. MD5 is not FIPS compliant. Using a FIPS compliant hashing algorithm method to generate a HostId will allow an NServiceBus endpoint to run under a FIPS policy.

First a replacement for the MD5 based DeterministicGuid utility within the NServiceBus framework will have to be provided:

snippet: SHA1DeterministicGuid

Next [set the HostId](/nservicebus/hosting/override-hostid.md) as part of the endpoint configuration:

snippet: HostIdFixer

## Disable enforcement of FIPS

ServiceControl also uses MD5 internally and will require disabling FIPS enforcement to run properly.

FIPS enforcement can be disabled by setting the runtime setting `enforceFIPSPolicy` to `false` in the applications app.config or web.config. See the [MSDN article on how to change this setting](https://msdn.microsoft.com/en-us/library/hh202806(v=vs.110).aspx).
