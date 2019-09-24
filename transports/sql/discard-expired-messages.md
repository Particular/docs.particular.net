---
title: Discarding expired messages
reviewed: 2019-08-21
component: SqlTransport
---

## Sending

Messages with Time-To-Be-Received (TTBR) set are stored in the queue table just like regular messages. The only difference is that the `Expires` column is set to the time (in UTC) when the message expires. The calculation of expiry time is done on the database level so that clock drifts between endpoints don't affect the TTBR.

## Receiving

partial: receiving
