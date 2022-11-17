It is possible to create a hierarchy of topics by separating the publish topic from the subscribe topic. Due to the added complexity of the hierarchy and the increased number of operations, it is encouraged to start with a single-topic topology and reevaluate options when approximately 50% of the capacity is reached.

For example, an existing system using `bundle-1` as the default single topic topology can add new endpoints as subscribers under `bundle-2` but continue publishing all their events to `bundle-1`. This ensures non-interrupted communication between the endpoints while making it possible to scale beyond the limits of a single topic. A newly added endpoint would need to configure its topology as follows:

snippet: custom-topology-hierarchy-bundle

This creates a new `bundle-2` topic, a subscription called `forward-bundle-2` on the `bundle-1`topic, which automatically forwards all events published to `bundle-1` to `bundle-2`. The new endpoint creates its subscription under `bundle-2` as shown in the picture below:

![Topology Hierarchy](forwarding-topology-hierarchy.svg "width=500")

NOTE: While it is technically possible to create even deeper hierarchies, it is strongly discouraged to do so due to the complexity and limitations of the number of hops a message can be routed through. The maximum allowed number of hops in Azure Service Bus is four and needs to be considered when creating a topology hierarchy. In addition, introducing more forwarding hops also increases the number of operations used, affecting pricing or memory/CPU used depending on the selected Azure Service Bus tier.