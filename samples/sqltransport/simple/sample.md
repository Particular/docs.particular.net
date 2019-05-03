---
title: Simple SQL Server Transport Usage
summary: A simple send and receive scenario with the SQL Server Transport.
reviewed: 2019-05-03
component: SqlTransport
related:
- transports/sql
---


## Prerequisites

include: sql-prereq

The database created by this sample is called `NsbSamplesSqlTransport`.


## Running the project

 1. Start both the Sender and Receiver projects
 1. At startup Sender will send a message to Receiver.
 1. Receiver will handle the message.


## Code walk-through


### Configure the SQL Server transport

snippet: TransportConfiguration
