---
title: Security Advisory 2020-03-11
summary: ServicePulse directory traversal vulnerability
reviewed: 2020-03-11
---

This advisory discloses a security vulnerability that has been found in [ServicePulse](/servicepulse/) and fixed in a recently released version.

 * ServicePulse versions 1.24 or 1.24.1 should be upgraded to the latest version of ServicePulse to fix this vulnerability.
 * Users using [IIS to host ServicePulse](/servicepulse/install-servicepulse-in-iis.md) are not affected

If there are any questions or concerns regarding this advisory, send an email to [security@particular.net](mailto://security@particular.net).

## ServicePulse directory traversal vulnerability

A vulnerability has been fixed in the ServicePulse host application that allows attackers to access files using malicious URLs.

### Impact

Attackers can use this vulnerability to read any file the ServicePulse host process has access to. The default user account for new ServicePulse installations is _Local Service_, which has access to the files on the host system.

### Exploitability

The exploitation of this vulnerability requires the attacker to have access to the ServicePulse web application.

### Affected versions

Versions 1.24 and 1.24.1 of ServicePulse are affected by this vulnerability.

### Risk mitigation

If it is not possible to immediately upgrade ServicePulse to the latest version, the following approach can be used as a **temporary workaround:**

- Change the user account running the ServicePulse service to a user with file access restricted to files located in the ServicePulse installation folder only, by default `C:\Program Files (x86)\Particular Software\ServicePulse`.

### Fix

This vulnerability can be fixed by upgrading ServicePulse to the latest version. Upgrades should be performed as follows:

[Download](https://particular.net/start-servicepulse-download) and run the latest version of the ServicePulse installer, following the online [installation instructions](/servicepulse/installation.md#installation)


### Contact info

If there are any questions or concerns regarding this advisory, contact [security@particular.net](mailto://security@particular.net).