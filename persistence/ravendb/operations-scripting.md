---
title: Scripting
summary: Example code and scripts to facilitate deployment and operational actions against RavenDB.
reviewed: 2016-10-18
related:
 - nservicebus/operations
redirects:
 - nservicebus/ravendb/operations-scripting
---

Example code and scripts to facilitate deployment and operational actions against RavenDB.

These examples use the [RavenDB.Client](https://www.nuget.org/packages/RavenDB.Client/) NuGet.


## Grant a user access to a database


### The user access helper method

The following code shows an example of how to grant a user access to a RavenDB database.

This is helpful to ensure the user account, an endpoint is running under, has appropriate access to RavenDB.

snippet: raven-add-user


### Using the user access helper method

snippet: raven-add-user-usage
