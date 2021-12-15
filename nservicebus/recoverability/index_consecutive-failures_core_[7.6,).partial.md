## Automatic rate limiting

The automatic rate limiting in response to consecutive message processing failures is designed to act as an [automatic circuit breaker](https://en.wikipedia.org/wiki/Circuit_breaker) preventing large number of messages from being redirected to the `error` queue in case of an outage of a resource required for processing of all messages (e.g. a database).

The following code enables the detection of the consecutive failures.

snippet: configure-consecutive-failures

When the endpoint detects a configured number of consecutive failures, it reacts by switching to a single-threaded processing mode in which one message is attempted at a time. If processing fails, the endpoint waits for configured time and attempts to process the next message. The endpoint continues running in this mode until at least one message is processed successfully.