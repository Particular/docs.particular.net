{{WARNING:
This serializer does not support [messages defined as interfaces](/nservicebus/messaging/messages-as-interfaces.md). If an explicit interface is sent, an exception will be thrown with the following message:

```
Interface based message are not supported.
Create a class that implements the desired interface
```

Instead, use a public class with the same contract as the interface. The class can optionally implement any required interfaces.
}}
