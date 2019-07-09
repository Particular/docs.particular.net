---
title: Configure the ServiceControl Audit instance URI
summary: How to configure a ServiceControl Audit instance to be exposed through a custom hostname and IP port
reviewed: 2019-07-08
---


## Changing the ServiceControl Audit instance URI

This article describes the steps required to set a custom hostname and IP port for an instance of the ServiceControl Audit HTTP API.

NOTE: Anyone who can access the ServiceControl Audit instance URL has complete access to the endpoint data stored by the ServiceControl Audit instance. This is  why the default is to only respond to `localhost`. Consider carefully the implications of exposing a ServiceControl Audit instance via a custom or wildcard URI.


### Using ServiceControl Management

 1. Click the Configuration Icon for for the ServiceControl Audit instance to modify.
 1. Change the Host Name and Port Number fields to the desired values.
 1. Click Save.

ServiceControl Management will then validate the settings changes and restart the service to apply the changes.

TODO: Mention updating ServiceControl Remotes