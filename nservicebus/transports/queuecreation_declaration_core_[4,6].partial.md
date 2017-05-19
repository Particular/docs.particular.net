
## Declaration

Queues should be declared during the Setup phase of a [Feature](/nservicebus/pipeline/features.md).

A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

snippet: queuebindings
