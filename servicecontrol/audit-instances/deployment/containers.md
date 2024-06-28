---
title: Deploying ServiceControl Audit instances using containers
reviewed: 2024-06-11
component: ServiceControl
---

ServiceControl audit instances can be run as a container. They are hosted on the Docker hub. https://hub.docker.com/u/particular

## Basic usage

```bash
docker run -e TRANSPORTTYPE=ServiceControl.Transports.Learning particular/servicecontrol-audit:latest
```

## Dependent Infrastructure

### RavenDB

include: ravendb-dependency

### Error instance

ServiceControl audit requires an error instance to connect to.

Example compose file:

```yaml
services:
  audit:
  error:
```

## Required environment variables

| Variable | |
| -- | -- |
| | |

See settings for other configuration options that can be set via environment variables.

## Running in production

The one volume needed. Client certificate. Could have settings file. Could have license file.

## Tagging

ServiceControl audit instances on Docker Hub are tagged as follows:

### Latest

The latest pushed version number of ServiceControl will be tagged as `:latest`.

### Major

The latest pushed version number within a given major will be tagged with the major version number: e.g. version `5.3.2` will be tagged with `:5`. This allows you to lock in a major version avoiding any breaking changes but still get any critical patch releases. See the Support Policy.

### Version

Every image uploaded will be tagged with the release version number, e.g. `:5.3.2`.

## Init containers

The `init` containers are used to create or upgrade the infrastructure required for ServiceControl. They are based on the [Kubernetes `init` containers](https://kubernetes.io/docs/concepts/workloads/pods/init-containers/). Once the `init` container has been run, it will shut down and the `runtime` container can be run.

`Init` containers are identical to the corresponding `runtime` image but with added entry point arguments (`--setup`). It is also possible to use the `runtime` image, override the entry point and add this argument to achieve the same goal.

```bash
docker run -rm blah blah --setup
```

## Upgrading

```bash
docker stop audit
docker rm audit
docker run servicecontrol-audit:latest
```

## Maintenance mode

```bash
docker stop audit
```

```bash
docker start audit
```
