---
title: How to Discard Old Messages?
summary: Automatically discard messages if they have not been processed within a given period of time.
originalUrl: http://www.particular.net/articles/how-do-i-discard-old-messages
tags:
- Message Expiration
- TimeToBeReceived
createdDate: 2013-05-21T11:35:06Z
modifiedDate: 2013-11-20T17:58:29Z
authors: []
reviewers: []
contributors: []
---


If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, you may want to discard the message.

To discard a message when a specific time interval has elapsed:

```C#
[TimeToBeReceived("00:01:00")] // Discard after one minute
[Recoverable]
public class MyMessage { }
```