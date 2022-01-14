## Automatic rate limiting

The automatic rate limiting in response to consecutive message processing failures is designed to act as an [automatic circuit breaker](https://en.wikipedia.org/wiki/Circuit_breaker) preventing a large number of messages from being redirected to the `error` queue in the case of an outage of a resource required for processing of all messages (e.g. a database or a third-party service).

The following code enables the detection of consecutive failures.

snippet: configure-consecutive-failures

When the endpoint detects a configured number of consecutive failures, it reacts by switching to a processing mode in which one message is attempted at a time. If processing fails, the endpoint waits for configured time and attempts to process the next message. The endpoint continues running in this mode until at least one message is processed successfully.

### Considerations when configuring automatic rate limiting

1. The number of consecutive failures must be big enough so that it doesn't trigger rate-limiting when only a few failed messages are processed by the endpoint.
2. Endpoints that process many different message types may not be a good candidates for this feature. When rate limiting is active, it affects the entire endpoint. Endpoints that are rate limited due to a failure for one message type will slow down processing of all message types handled by the endpoint.
