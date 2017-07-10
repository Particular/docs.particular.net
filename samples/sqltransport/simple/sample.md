---
title: Simple SQL Server Transport Usage
summary: A simple send and receive scenario with the SQL Server Transport.
reviewed: 2016-04-27
component: SqlTransport
related:
- transports/sql
---


## Prerequisites

 1. Ensure an instance of SQL Server Express is installed and accessible as `.\SQLEXPRESS`. Create a database `SqlServerSimple`.


## Running the project

 1. Start both the Sender and Receiver projects
 1. At startup Sender will send a message to Receiver.
 1. Receiver will handle the message.


## Code Walk-through


### Configure the SQL Server Transport

snippet: TransportConfiguration
