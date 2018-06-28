When using a transport that doesn't support publish-subscribe natvely, if a message handler is defined for an event but no publishers information can be found the endpoint will log an error, as the following one, at startup:

```
AutoSubscribe was unable to subscribe to event '<event-type-full-name>': No publisher address could be found for message type '<event-type-full-name>'.
```