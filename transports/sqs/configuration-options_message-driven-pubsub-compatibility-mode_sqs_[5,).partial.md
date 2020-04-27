### Message driven Pub/Sub compatibility mode

To gradually move an existing system from message driven pub/sub to native pub/sub using SNS it's possible to enable the message driven Pub/Sub compatibility mode.

Message driven Pub/Sub compatibility mode needs to be enabled on publisher endpoints. When enabled publishers will still consume subscription messages sent by endpoints using message driven pub/sub and when publishing an event it'll be published both to legacy subscribers and to SNS. Publishers deduplicates published events.

To enable the message driven Pub/Sub compatibility mode configure the endpoint as follows:

snippet: EnableMessageDrivenPubSubCompatibilityMode