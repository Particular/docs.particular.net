The pipeline is broken down into smaller composable units called *Stages* and *Connectors*. A stage is a group of steps acting on the same level of abstraction.

Note: There currently is no way to provide custom Pipelines, Stages, Fork Connectors, or Stage Connectors. Existing Stages, Fork Connectors, or Stage Connectors can be replaced but extreme caution has to be applied. Stages, Fork Connectors and Stage Connectors define the inner workings of NServiceBus core behavior. Wrongly replaced stages or connectors could lead to message loss. The examples below are included here for completeness.


## Stages

The pipeline consists of three main parts: incoming, outgoing and recoverability. Both parts are comprised from a number of stages.

The following lists describe some of the common stages that behaviors can be built for. Each stage has a context associated with it (which is used when implementing a custom behavior).

NOTE: Data can be added to, and retrieved from, the context with the `Extensions` property of type `ContextBag`. Each following stage has access to data set in a previous stage but data set in a later stage is not available in a prior stage. The context bag is cloned at each stage transition and is not threadsafe.

In the diagram User Code can refer to a handler or a saga. If the handler or saga sends a message, publishes an event, or replies to a message, then the details from the incoming message will be added to the outgoing context.