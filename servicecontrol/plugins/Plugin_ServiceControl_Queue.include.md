Plugins send messages using the defined endpoint transport to ServiceControl. The plugin must be configured with the location of the ServiceControl input queue. This queue is created during the [installation of ServiceControl](/servicecontrol/installation.md). It is based on the Windows Service name.

Configure the ServiceControl queue via config:

snippet: sc-plugin-queue-config