### Prefixing the delayed delivery infrastructure

It is possible to append a prefix to the queues and exchanges used for delayed delivery.

snippet: rabbitmq-delay-specify-prefix

Using the example above, all of the delayed delivery queues and exchanges will be prefixed by `systemA`, e.g. `systemA.nsb.delay-level-00`.

WARN: This is a system-wide setting. All endpoints must use the same prefix.