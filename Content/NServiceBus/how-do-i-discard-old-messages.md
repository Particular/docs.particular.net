---
title: "How to Discard Old Messages?"
tags: ""
summary: "If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, the message is discarded."
---


If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, the message is discarded.


```C#
[TimeToBeReceived("00:01:00")] // Discard after one minute
[Recoverable]
public class MyMessage { }
```



<div id="rate_article_container">
<div id="rate_article">






