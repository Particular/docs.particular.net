## Automatic rate limiting

The automatic rate limiting in response to consecutive message processing failures is designed to act as an [automatic circuit breaker](https://en.wikipedia.org/wiki/Circuit_breaker) preventing large number of messages from being redirected to the `error` queue in case of an outage of a resource required for processing of all messages (e.g. a database or a 3rd party webservice).

The following code enables the detection of the consecutive failures.

snippet: configure-consecutive-failures

When the endpoint detects a configured number of consecutive failures, it reacts by switching to a single-threaded processing mode in which one message is attempted at a time. If processing fails, the endpoint waits for configured time and attempts to process the next message. The endpoint continues running in this mode until at least one message is processed successfully.

### What to consider configuring automatic rate limiting

1. The number of consecutive failure need to be big enough to not trigger rate-limiting when a couple of failed messages are processed by the endpoint. 
2. Endpoint that process many different message types might not be a good candidates for this feature. 
The reasoning is, if one message type is failing (due to code bug in the handler) the endpoint will frequently tunr on and off the automatic rate limiting, effectivelly slowing down processing of the other message types.
