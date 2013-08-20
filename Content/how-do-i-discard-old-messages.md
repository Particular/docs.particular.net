<!--
title: "How to Discard Old Messages?"
tags: 
-->

    TimeToBeReceived(“00:01:00”)] // one minute
    [Recoverable]
    public class MyMessage { }

If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, the message is discarded.


<div id="rate_article_container">
<div id="rate_article">






