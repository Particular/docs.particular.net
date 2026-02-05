---
title: FIPS Compliance
summary: Describes NServiceBus compliance with FIPS
component: Core
reviewed: 2025-12-19
---

The [Federal Information Processing Standards](https://en.wikipedia.org/wiki/Federal_Information_Processing_Standards) or FIPS are standards developed by the United States government for computer systems that set requirements for, among other things, cryptography.

Microsoft [no longer recommends enabling FIPS](https://web.archive.org/web/20190419143230/https://blogs.technet.microsoft.com/secguide/2014/04/07/why-were-not-recommending-fips-mode-anymore/) unless it is required by government regulations.

> [!NOTE]
> FIPS policy enforcement does only exist on .NET Framework.

The Particular Software Platform is not FIPS compatible, and no testing is done to ensure components will work properly on FIPS-enabled hardware. The platform currently uses `System.Security.Cryptography` classes only for hashing, and not for data security purposes.

There are workarounds that allow running NServiceBus and the Particular Service Platform on the .NET Framework on servers with FIPS enforcement enabled, but these workarounds are also not tested or verified in any way.

## NServiceBus

NServiceBus uses the MD5 hash algorithm to generate deterministic unique identifiers for endpoints, also known as HostIds. MD5 is not FIPS compliant. Using a FIPS compliant hashing algorithm method to generate a HostId will allow an NServiceBus endpoint to run under a FIPS policy.

First, a replacement for the MD5 based DeterministicGuid utility within the NServiceBus framework will have to be provided:

snippet: SHA1DeterministicGuid

Next, [set the HostId](/nservicebus/hosting/override-hostid.md) as part of the endpoint configuration:

snippet: HostIdFixer

## Component libraries

The following packages use MD5 and cannot be used with FIPS enforcement enabled:

* NServiceBus.RavenDB - Uses MD5 to create shortened keys for subscriptions and saga lookup properties.
* NServiceBus.Gateway - Uses MD5 to ensure integrity of received data.
* NServiceBus.Distributor.Msmq: Uses MD5 to shorten long queue names.

## Disable enforcement of FIPS

ServiceControl and ServicePulse also use MD5 internally and will require disabling FIPS enforcement to run properly. As these tools do not execute user code and can be audited as 100% open source, it is sometimes possible to obtain a waiver to run these tools with a configuration flag that instructs the .NET Framework to skip enforcement of FIPS even when configured to do so at the server level with group policy.

FIPS enforcement can be disabled by setting the runtime setting `enforceFIPSPolicy` to `false` in the application's app.config or web.config. See the [MSDN article on how to change this setting](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/runtime/enforcefipspolicy-element).
