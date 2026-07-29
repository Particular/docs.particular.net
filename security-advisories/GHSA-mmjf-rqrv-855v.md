## Security Advisory Id GHSA-mmjf-rqrv-855v

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-mmjf-rqrv-855v](https://github.com/advisories/ghsa-mmjf-rqrv-855v) security advisory: Microsoft Security Advisory CVE-2026-50527 &#8211; .NET Denial of Service Vulnerability.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|ServiceInsight|2.13.3|[The Download Page](https://particular.net/downloads)|


### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
