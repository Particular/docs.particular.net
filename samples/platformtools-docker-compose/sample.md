---
title: Host platform tools using Docker for Windows
summary: Host the ServicePulse and ServiceControl platform tools using Docker Windows Containers for Server and Desktops
reviewed: 2020-12-14
component: ServiceControl
related:
- samples/hosting/docker
---

NOTE: This sample is **not production ready**. Ports in the containers are remotely accessible and the sample is targeted to developers.

This sample shows how to host the [ServicePulse](/servicepulse/) and [ServiceControl](/servicecontrol/) platform tools in Docker Windows Containers for Server and Desktops . It makes use of `docker-compose` to set up all platform tool components.

## Prerequisites

- Ensure that Docker has been installed either for [Windows 10](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-10-Client) or [Windows Server](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-Server).
- A valid license file must be available at `C:\ProgramData\ParticularSoftware\license.xml`.
- An Azure Service Bus connection string must be set in an environment variables `SERVICECONTROL_CONNECTIONSTRING`, `SERVICECONTROL_AUDIT_CONNECTIONSTRING`, and `MONITORING_CONNECTIONSTRING`

## Windows vs Linux

Currently Linux is unsupported as ServiceControl has a technical dependency on ESENT storage which is only available on Windows.

A compose file cannot setup both Windows and Linux containers. [ServicePulse supports both Windows and Linux containerization](/servicepulse/containerization/).

## Memory

For this sample the containers will use the default limit of 1GB for containers used by Docker for Windows. This is sufficient for demo purposes but not when ServiceControl is under load or when the database grows in size. This limit can be adjusted by uncommenting the text `#mem_limit: 8192m` in the Docker Compose files.

## License

As the platform tools are licensed software, a license file is required to run the tools in containers. The license file in the sample assumes the default path: `C:\ProgramData\ParticularSoftware\license.xml`.  Alternatively, set in the `.env` environment variables file with values for `SERVICECONTROL_LICENSETEXT`, `SERVICECONTROL_AUDIT_LICENSETEXT`, and `MONITORING_LICENSETEXT`.

## Storage

ServiceControl requires storage for its database. Data stored by ServiceControl must be persistent and not stored in the container itself as containers often need to be rebuilt. ServiceControl data is stored via [docker volumes](https://docs.docker.com/storage/volumes/) which are resilient to container rebuilds so that data is not lost. This sample writes logs to a docker volume as well to ensure logs are not lost.

## Transport

The [Azure Service Bus Transport](/transports/azure-service-bus/) is used but Docker works with all supported [broker-based transports](/transports/selecting.md#broker-versus-federated).

Set the Azure Service Bus connection string in the `.env` file (value `Endpoint=sb://xxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxx`).

## Init and runtime containers

ServiceControl has a setup and a run-time stage. During the setup stage, queues are created but no messages are ingested and processed while during the run-time stage no setup is performed and messages are ingested. In a production environment often the setup stage is run with administrative access to resources and the runtime stage is run with least privilege.

The same stages are applied to Docker. The `docker-compose.init.yml` Docker Compose file executes the [ServiceControl init containers](/servicecontrol/containerization/#init-containers).

NOTE: The init and runtime Compose files should use different connection strings (administrative vs least privilege) in a non-developer environment.

## Running the sample

The init containers must be run before the runtime containers.

### Init

Runs Docker Compose and waits until all init containers have completed running. These should automatically exist after all setup logic completed.

NOTE: The command below omits the `-d, --detach` argument to ensure issues are written to the console. Alternatively, any issues are also visible in the container logs.

```cmd
docker-compose --file docker-compose.init.yml up
```

### Run

Runs Docker Compose and launch ServicePulse via the configured default browser. This uses the default Docker Compose file `docker-compose.yml`.

```cmd
docker-compose up --detach
start http://localhost:9090
```

### Maintenance mode

ServiceControl maintenance mode is demonstrated via the Docker Compose file `docker-compose.maintenance.yml`.

```cmd
docker-compose --file docker-compose.maintenance.yml up
```

### Teardown

Gracefully stops and removes the containers and the configured volumes.

### Updating

The Docker images follow [semantic versioning](https://semver.org/). In other words, breaking changes are introduced only in new major versions. Releases are pushed as `major.minor.patch` and it is safe to follow a `major` tag to ensure updates.

NOTE: Following the `latest` tag is recommended only for developers in combination with recreating Docker volumes via `docker compose up -d -V`.

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
