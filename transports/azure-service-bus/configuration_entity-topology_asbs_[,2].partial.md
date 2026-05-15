### Topology

* `TopicName(string)`: The topic's name used to publish events between endpoints. All endpoints share this topic, so ensure all endpoints specify the same topic name. Defaults to `bundle-1`. Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://learn.microsoft.com/en-us/rest/api/servicebus/create-topic).