---
title: SQL Server Transport Queue Peek Settings
reviewed: 2016-09-01
component: SqlServer
versions: '[3.0,)'
tags:
 - SQL Server
---

The SQL Server transport uses database tables as conceptual queues to read messages from. Current implementation uses a pull mechanism to 'Peek' if there are any messages waiting to get processed. There is a delay between each peek which defaults to one second. Depending on the system, the amount of messages and the load, the peek delay time may be tweaked. 

The acceptable range for this setting is between 100 milliseconds to 10 seconds. If a value higher than the maximum recommended settings is used, a warning message will be logged.

## Delay setting configuration

Use the `WithDelay` extension method:

snippet:sqlserver-config-delay

