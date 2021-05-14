---
title: Running ServicePulse in containers
reviewed: 2020-12-17
---

Docker images for ServicePulse exist on Dockerhub under the [Particular organization](https://hub.docker.com/u/particular) and available for Windows and Linux.

## Containers overview

ServicePulse is stateless and requires no volume mapping. The UI fully runs in the client browser which connects directly to the ServiceControl API. Therefore it does not need any initialization.

## Ports

Port 80 is used for serving the ServicePulse web application

## Environment

ServicePulse is available as a Linux and as a Windows image

Linux:

The [particular/servicepulse](https://hub.docker.com/r/particular/servicepulse) image is based on `Ubuntu:latest`.

Windows:

The  [particular/servicepulse-windows](https://hub.docker.com/r/particular/servicepulse) image is based on `mcr.microsoft.com/windows/servercore/iis`.

## Running using Docker

Linux:

Host ServicePulse on Ubuntu Linux via Nginx run:

```cmd
docker run -p 80:90 --detach particular/servicepulse:1
```

Windows:

Host ServicePulse on Windows 2016 or later (Windows 10, Windows 2019) via IIS run:

```cmd
docker run -p 80:80 --detach particular/servicepulse-windows:1
```
