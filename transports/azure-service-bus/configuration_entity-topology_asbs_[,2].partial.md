### Topology

* `TopicName(string)`: The name of the topic used to publish events between endpoints. This topic is shared by all endpoints, so ensure all endpoints configure the same topic name. Defaults to `bundle-1`. Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).