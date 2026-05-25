---
title: Security Advisories
reviewed: 2026-05-22
suppressRelated: true
---

* [Security Advisory 2022-12-06](cryptography-xml-vulnerability.md)
  * System.Security.Cryptography.Xml vulnerability (CVE-2022-34716) in NServiceBus
* [Security Advisory 2020-03-11](servicepulse-directorytraversal.md)
  * ServicePulse Directory Traversal vulnerability.
* [Security Advisory 2017-01-10](msmq-permissions.md)
  * MSMQ permissions vulnerability.
* [Security Advisory 2016-07-05](sqlserver-sqlinjection.md)
  * NServiceBus SQL Server Transport injection vulnerability.

## CVE Handling Process

Particular Software maintains an automated process to identify, assess, track, and remediate vulnerabilities affecting both direct and transitive software dependencies used in its repositories.

### 1. Detection

Automated dependency auditing is used to detect known vulnerable packages, including vulnerabilities introduced through transitive dependencies. Detection is based on published vulnerability intelligence associated with package ecosystems and GitHub Security Advisory identifiers.

When a vulnerability is detected, the automation records the advisory using its unique `GitHubAdvisoryId` and begins coordinated tracking.

### 2. Centralized internal triage

For each newly detected dependency vulnerability, a centralized internal tracking issue is created that serves as the coordination point for security review, prioritization, remediation planning, and status tracking.

### 3. Repository and branch impact assessment

After initial recording, the affected repositories and versions are determined. For each affected repository/version combination, the system creates a GitHub issue labeled as `Dependency CVE` that includes the advisory reference and necessary context.
Customers can subscribe to these issues to receive updates on the vulnerability status and remediation progress.

If customers cannot wait for a fix to be released, they can [pin transitive dependencies](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages#transitive-packages) until a fix is released.

### 4. Remediation

Dependency vulnerabilities are remediated by one or more of the following actions:

- upgrading the affected package to a version that resolves the vulnerability
- upgrading a parent dependency that resolves the transitive vulnerability
- removing the vulnerable dependency path
- applying another documented mitigation where no fixed version is yet available

### 5. Disclosure and external communication

For dependency vulnerabilities discovered through public advisories, the upstream advisory and CVE record are treated as the authoritative public source for the vulnerability.

Where customer-facing communication is needed, the following will be disclosed:

- the affected product or repository scope
- the relevant advisory or CVE identifier
- affected versions
- remediation or mitigation guidance
- fix availability status

