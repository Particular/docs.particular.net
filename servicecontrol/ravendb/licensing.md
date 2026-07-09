---
title: Licensing the ServiceControl RavenDB database
summary: How the RavenDB database used by ServiceControl is licensed, the connectivity it uses, and how to run it in air-gapped deployments
reviewed: 2026-07-09
component: ServiceControl
versions: '[5,)'
related:
- servicecontrol/license
- servicecontrol/ravendb/containers
---

ServiceControl stores its data in [RavenDB](https://ravendb.net/). The RavenDB server is hosted in one of two ways:

- as a **child process** started by ServiceControl, using the RavenDB server and license bundled with the ServiceControl installation — the default for Windows and other non-container installations; or
- as a **container**, using the [`particular/servicecontrol-ravendb` image](/servicecontrol/ravendb/containers.md).

In both cases the RavenDB license is the `RavenLicense.json` file bundled with ServiceControl and is covered by the [Particular Software license](/servicecontrol/license.md) — a separate RavenDB license is not required. This page describes how that license is kept valid, the connectivity RavenDB uses, and how to run ServiceControl in air-gapped or egress-restricted environments.

## How the license is kept valid

The RavenDB server validates its license against the RavenDB server **version** that is running: a license covers server builds released within its update window. The RavenDB server also refreshes its license from the RavenDB license server at `api.ravendb.net`.

When the bundled license already covers the running server version — the normal case, because ServiceControl ships a matched RavenDB server and license — the license validates without contacting the license server. A refresh from `api.ravendb.net` is only needed when the running server version is **newer** than the locally available license covers.

## Outbound connectivity

To validate and refresh its license, the RavenDB server contacts the RavenDB license server, using outbound **HTTPS (port 443)** with name resolution, at:

```
api.ravendb.net
```

This is needed to obtain an updated license when the one held locally does not cover the running server version (see [Air-gapped and egress-restricted deployments](#air-gapped-and-egress-restricted-deployments)). ServiceControl itself does not require internet access — the [Particular Software license](/servicecontrol/license.md) is supplied directly to each instance.

## Air-gapped and egress-restricted deployments

If the RavenDB server cannot reach `api.ravendb.net` **and** the license available to it does not cover the RavenDB server version that is running, it cannot validate its license. When this happens:

- The **RavenDB server keeps running**, but in a restricted, unlicensed mode.
- The **ServiceControl instance connected to it fails to start**. In ServiceControl version 6.9 and later, the instance stops immediately with a fatal error rather than continuing in a degraded state:

```
Cannot validate the current RavenDB license
```

This requires the RavenDB server version and its license to have diverged, which is primarily a concern for the **container** deployment: the [`particular/servicecontrol-ravendb` image](/servicecontrol/ravendb/containers.md) is versioned and pulled independently of ServiceControl, so a database container can end up running a server version that its bundled license does not cover — most commonly after upgrading the database image without a matching license, or when different instances in a fleet run different image versions.

Installations that use the **bundled** RavenDB server (the child process on Windows and other non-container hosts) install the RavenDB server and its license together, so they always match and validate offline. This failure does not arise from a normal installation or upgrade of such a host.

## Remedies

### Restore access to the license server

The recommended remedy is to allow the RavenDB server outbound access to `api.ravendb.net`, as described in [Outbound connectivity](#outbound-connectivity). Once connectivity is available, restart the affected ServiceControl instance; the RavenDB server refreshes its license during startup. The license can also be refreshed from the **License** view in [RavenDB Studio](/servicecontrol/ravendb/accessing-database.md) using **Force Update**.

### Air-gapped deployments

When outbound access to `api.ravendb.net` cannot be granted, keep the RavenDB server and its license matched and current:

- **Container** — run an up-to-date [`particular/servicecontrol-ravendb` image](/servicecontrol/ravendb/containers.md) matched to the ServiceControl version. Because the license is included in the image, a current, matched image already contains a license that covers its own server version, so no refresh from the license server is required.
- **Bundled (child process)** — no separate action is required. Upgrading ServiceControl installs a matched RavenDB server and license together, so the license always covers the server version being run.

## Version compatibility

When the database runs in a container or as a separate server, the RavenDB database container should use the **same version** (`Major.Minor.Patch`) as the ServiceControl instances that connect to it. See the version map in [Managing ServiceControl RavenDB instances via Containers](/servicecontrol/ravendb/containers.md).

A ServiceControl instance can connect to a RavenDB server of the **same or a higher** RavenDB version, because RavenDB clients are compatible with servers of an equal or higher version. An older ServiceControl instance can therefore operate against a newer RavenDB server, for example during a staged upgrade. A ServiceControl instance refuses to start only when it requires a **newer** RavenDB server version than the one that is running.
