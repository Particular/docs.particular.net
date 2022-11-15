It is possible to create a hierarchy of topics by separating the publish topic from the subscribe topic. Due to the additional complexity of the hierarchy and the involved increase in the number of operations it is encouraged to start with a single topic topology and revaluate options when approximately 50% of the capacity is reached.

For example an existing system using `bundle-1` as the default single topic topology can add new endpoints as subscribers under `bundle-2` but let them still publish all their events to `bundle-1` ensuring non-interrupted communication between the endpoints while making it possible to scale beyond the limits of a single topic. A newly added endpoint would need to configure a topology in the following way:

snippet: custom-topology-hierarchy-bundle

This would then create a new `bundle-2` topic, a suscription called `forward-bundle-2` on the `bundle-1`topic that automatically forwards all events published to `bundle-1` to `bundle-2`. The newly added endpoint would then create its subscription under `bundle-2` as shown in the picture below:

![Topology Hierarchy](forwarding-topology-hierarchy.svg "width=500")

NOTE: While it is technically possible to create even deeper hierarchies it is strongly discourage to do so due to the complexity and limitations with the number of hops a message can be routed through. The maximum allowed number of hops Azure Service Bus is four and needs to be taken into consideration when creating a topology hierarchy. On top of that introducing more forwarding hops also increases the number of operations being used which can have an effect on the pricing or memory/cpu being used depending on the selected Azure Service Bus tier.