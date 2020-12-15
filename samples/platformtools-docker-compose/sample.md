---
title: Host platform tools using Docker for Windows
summary: Host the ServicePulse and ServiceControl platform tools using Docker Windows Containers for Server and Desktops
reviewed: 2020-12-14
component: ServiceControl
related:
- samples/hosting/docker
---

NOTE: This sample is **not production ready** as ports are remotely accessible and it is targeted to developers.

This sample shows how to host the [ServicePulse](/servicepulse/) and [ServiceControl](/servicecontrol/) platform tools using Docker Windows Containers for Server and Desktops . It makes use of `docker-compose` to easily setup all platform tool components.

## Prerequisites

- Ensure that Docker has been installed either for [Windows 10](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-10-Client) or [Windows Server](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-Server).
- Valid license file available at `C:\ProgramData\ParticularSoftware\license.xml`.
- Azure Service Bus connection string set in environment variable `AZURESERVICEBUS_CONNECTIONSTRING`

## Windows vs Linux

Currently Linux is unsupported as ServiceControl has a technical dependancy on ESENT storage which is only available on Windows.

A compose file cannot setup both Windows and Linux containers at this writing. [ServicePulse supports both Windows and Linux containerization](/servicepulse/containerization/).

## License

As the platform tools are licensed software the sofware needs a license file. The license file in the sample assumes the default path `C:\ProgramData\ParticularSoftware\license.xml`.

## Storage

ServiceControl requires storage. Data stored by ServiceControl must be persistent and not stored in the container itself as containers often need to be rebuild. ServiceControl Data is stored via [docker volumes](https://docs.docker.com/storage/volumes/) which is resilient to container rebuilds so that data is not lost. This sample writes logs to a docker volume too to ensure logs are not lost.

## Transport

The [Azure Service Bus Transport](/transports/azure-service-bus/) is used but docker works well with all supported [broker based transports](/transports/selecting.md#broker-versus-federated).

Set the Azure Service Bus connection string in the `environment.env` file (value `Endpoint=sb://xxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxx`).

## Init and Runtime containers

ServiceControl has a setup and a run-time stage. During the setup stage queues are created but no messages are ingested and processed while during the run-time stage no setup is performed and messages are ingested. In a production environment often the setup stage is run with administrative access to resouces and the runtime stage is run least priviledge.

The same stages are applied to docker. The `docker-compose.init.yml` docker compose file executes the [ServiceControl init containers](/servicecontrol/containerization/#init-containers).

NOTE: The init and runtime compose files would be using different connection strings (administrative vs least priviledge) in a non-developer environment.

## Running the sample

The init containers need to be run before the runtime containers.

### Init

Runs compose and wait until all init containers completed. This should automatically exist after all setup logic completed.

NOTE: This is ommitting the `-d, --detach` argument to ensure issues are writting to the console. Alternatively, any issues are visible in the container logs.

```cmd
docker-compose -f docker-compose.init.yml up
```

### Run

Runs compose and launch ServicePulse via the configured default browser.

```cmd
docker-compose -f docker-compose.runtime.yml up --detach
start http://localhost:9090
```

### Teardown

Gracefully stops and removes the containers and the configured volumes.

### Updating

The docker images are following semver. Meaning, breaking changes are only introduced in new majors. Releases are pushed as `major.minor.patch` and it is safe to follow a `major` tag to ensure updates.

NOTE: Following `latest` is only recommended for developers in combination with recreating docker volumes via `docker compose up -d -V`.

In order to update all containers to their latest versions:

```cmd
docker pull particular/servicecontrol.azureservicebus.init-windows:4
docker pull particular/servicecontrol.azureservicebus-windows:4
docker pull particular/servicecontrol.azureservicebus.monitoring.init-windows:4
docker pull particular/servicecontrol.azureservicebus.monitoring-windows:4
docker pull particular/servicecontrol.azureservicebus.audit.init-windows:4
docker pull particular/servicecontrol.azureservicebus.audit-windows:4
docker pull particular/servicepulse-windows:1
docker-compose -f docker-compose.runtime.yml up --detach
```
