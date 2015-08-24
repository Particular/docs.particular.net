---
title: One Way/Send Only Endpoints
summary: Use "Send only mode" for endpoints whose only purpose is sending messages
tags:
- Hosting
- SendOnly
- Web
redirects:
- nservicebus/one-way-send-only-endpoints
---

You would use this for endpoints whose only purpose is sending messages, such as websites. This is the code for starting an endpoint in send only mode.
 
<!-- import SendOnly -->

The only configuration when running in this mode is the destination of the messages you're sending. You can configure it [inline or through configuration](/nservicebus/messaging/specify-message-destination.md).
