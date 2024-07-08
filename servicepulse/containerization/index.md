---
title: Running ServicePulse in containers
reviewed: 2024-02-26
---

Docker images for ServicePulse exist on Docker Hub under the [Particular organization](https://hub.docker.com/u/particular) and are available for Windows and Linux.

## Containers overview

ServicePulse is stateless and requires no volume mapping. The UI fully runs in the client browser which connects directly to the ServiceControl API. Therefore it does not need any initialization.

The [particular/servicepulse](https://hub.docker.com/r/particular/servicepulse) image is based on `nginx:stable-alpine`.

## Ports

Port 90 is used for serving the ServicePulse web application.

## Running using Docker

Host ServicePulse on Ubuntu Linux via Nginx run:

```cmd
docker run -p 9090:90 -e SERVICECONTROL_URL="http://servicecontrol:33333/api/" -e MONITORING_URLS="['http://servicecontrol-monitoring:33633']" particular/servicepulse:latest
```

## Environment variables

- **`SERVICECONTROL_URL`**: _Default_: `http://localhost:33333/api/`. The url to your ServiceControl instance
- **`MONITORING_URLS`**: _Default_: `['http://localhost:33633/']`. A JSON array of URLs to your monitoring instances
- **`DEFAULT_ROUTE`**: _Default_: `/dashboard`. The default page that should be displayed when visiting the site
- **`SHOW_PENDING_RETRY`** _Default_: `false`. Set to `true` to show details of pending retries
