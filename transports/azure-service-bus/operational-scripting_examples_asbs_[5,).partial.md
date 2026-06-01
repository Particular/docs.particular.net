### Provisioning endpoints

Create the topology for an endpoint named `MyEndpoint`:

```txt
asb-transport endpoint create MyEndpoint -c "<connection-string>"
```

Create the topology for an endpoint named `MyEndpoint` and forward dead-lettered messages to the `error` queue:

```txt
asb-transport endpoint create MyEndpoint --forward-dlq-to error -c "<connection-string>"
```

Create a queue and configure dead-letter forwarding to the `error` queue:

```txt
asb-transport queue create MyEndpoint --forward-dlq-to error -c "<connection-string>"
```

### Subscribing to events (migration topology)

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` using the default settings:

```txt
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the subscription name to be `my-other-endpoint`

```txt
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -s my-other-endpoint -c "<connection-string>"
```

### Provisioning endpoints that use migration topology

Create the topology for an endpoint named `MyEndpoint` using the default settings:

```txt
asb-transport migration endpoint create MyEndpoint -c "<connection-string>"
```

Create migration topology and forward dead-lettered messages to the `error` queue:

```txt
asb-transport migration endpoint create MyEndpoint --forward-dlq-to error -c "<connection-string>"
```

Create the topology for an endpoint named `MyEndpoint` and override the topic name to be `custom-topic` and the subscription name to be `my-endpoint`:

```txt
asb-transport migration endpoint create MyEndpoint -t custom-topic -s my-endpoint -c "<connection-string>"
```

Create the topology for an endpoint named `MyEndpoint` and override the publish topic name to be `custom-publish-topic` and the subscription topic name to be `custom-subscribe-topic`:

```txt
asb-transport migration endpoint create MyEndpoint -tp custom-publish-topic -ts custom-subscribe-topic -c "<connection-string>"
```

### Subscribing to events

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` using the default settings:

```txt
asb-transport migration endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` using the topic-per-event apprach (after migration):

```txt
asb-transport migration endpoint subscribe-migrated MyOtherEndpoint Contracts.Events.SomeEvent -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the topic name to be `custom-topic`:

```txt
asb-transport migration endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -t custom-topic -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the subscription name to be `my-other-endpoint`

```txt
asb-transport migration endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -s my-other-endpoint -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the subscription rule name to be `SomeEvent`:

```txt
asb-transport migration endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -r SomeEvent -c "<connection-string>"
```
