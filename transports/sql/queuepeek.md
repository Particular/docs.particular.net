---
title: Queue Peek Settings
reviewed: 2016-09-01
component: SqlTransport
versions: '[3.0,)'
redirects:
- nservicebus/sqlserver/queuepeek
- transports/sqlserver/queuepeek
---

The SQL Server transport uses database tables as conceptual queues that it reads messages from. The implementation uses a pull mechanism to 'Peek' if there are any messages waiting to get processed.  Depending on the system, the amount of messages, and the load, the peek interval delay time may need to be tweaked. 

The default peek interval delay is 1 second. The recommended range for this setting is between 100 milliseconds to 10 seconds. If a value higher than the maximum recommended settings is used, a warning message will be logged. While a value less than 100 milliseconds will put too much unnecessary stress on the database, a value of larger than 10 seconds should also be use with caution as it may result in messages backing up in the queue. 

## Delay setting configuration

Use the following code:

snippet: sqlserver-config-delay
