---
title: Software supply chain
summary: How the integrity of software produced by Particular Software is maintained during the software development life cycle
reviewed: 2024-09-12
---

This document describes how the integrity of software produced by Particular Software is maintained during the software development life cycle.

## Systems access

* Multiple systems are used in the development life cycle, including GitHub, NuGet, and Microsoft Azure Key Vault.
* Multi-factor authentication is required for all services mentioned above and others.
* Only a limited subset of Particular Software employees act as administrators for each system.

## Software development

* All code is stored in [GitHub](https://github.com/Particular).
* Any code added to a project must be added via pull request.
* At least one other staff member must review a pull request before it can be merged to a release branch.
* Static code analysis during compilation enforces organizational coding conventions.

## Testing

* Automated test suites are run on code in every pull request branch.
* API verification tests ensure that breaking API changes cannot be introduced accidentally.
* Pull requests cannot be merged if the automated test suite fails.

## Deployment

* Merging a pull request does not immediately release new features to users, this requires an additional release step, as described below.
* All releases are signed with a code signing certificate:
  * The private key (RSA 4096 bits issued by DigiCert) is stored in a virtual hardware security module in [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/). The private key cannot be accessed by Particular Software staff, nor has it ever existed anywhere except in Key Vault.
  * Signing uses [NuGetKeyVaultSignTool](https://github.com/novotnyllc/NuGetKeyVaultSignTool) with a SHA256 hash.
  * The signing process occurs in Azure over a process protected by an OAuth login workflow.
  * Once signed, the contents of the package cannot be tampered with.
* All compiled software packages with associated source are available as GitHub releases.
* All GitHub releases are scanned for viruses.
  * Virus scanning uses the most recent version of ClamAV available from [apt-get](https://help.ubuntu.com/community/AptGet/Howto).
  * The virus database is updated immediately before scanning.
  * Both the compiled binaries and the source code that comprises them are scanned.
  * The output of the scan is appended to the GitHub release description.
* Compiled software libraries (such as NServiceBus) are published to NuGet.
  * Packages must be pushed to NuGet by a Particular staff member only after additional validation by the staff member.
  * NuGet will validate the package signature with Particular's public key to verify they were legitimately built by Particular Software and have not been compromised or tampered with.
  * Once on NuGet, the package is available for end users to update their own solutions.
  * End users still must take explicit action to upgrade after reviewing the package's release notes.
