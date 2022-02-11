NServiceBus has the concept of _pipelines_ which refers to the series of actions taken when:

* The incoming pipeline is triggered due to an incoming message that needs to be processed
* The outgoing pipeline is triggered due to an outgoing message that needs to be sent/published
* The [recoverability](/nservicebus/recoverability/) pipeline is triggered due to failure during processing 
