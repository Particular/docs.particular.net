NServiceBus uses the concept of _pipelines_. A pipeline refers to the series of actions taken as a result of the triggering action.

For example:

* The incoming pipeline is triggered due to an incoming message
* The outgoing pipeline is triggered due to an outgoing message
* The [recoverability](/nservicebus/recoverability/) pipeline is triggered due to failure during processing
