### Settings

* `EntityMaximumSize`: The maximum entity size in GB. The value must correspond to a valid value for the namespace type. Defaults to 5. See [the Microsoft documentation on quotas and limits](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas) for valid values.
* `EnablePartitioning`: Partitioned entities offer higher availability, reliability, and throughput over conventional non-partitioned queues and topics. For more information about partitioned entities [see the Microsoft documentation on partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning).
* `AutoDeleteOnIdle`: A `TimeSpan` representing the [AutoDeleteOnIdle](https://learn.microsoft.com/en-us/azure/service-bus-messaging/advanced-features-overview#autodelete-on-idle) setting for instance-specific input queues (such as when using [`MakeInstanceUniquelyAddressable`](/nservicebus/messaging/routing.md#make-instance-uniquely-addressable)) created by the transport. This value is the maximum time period that a queue can remain [idle](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-expiration#idleness) before Azure Service Bus automatically deletes it. Defaults to `TimeSpan.MaxValue` in Azure Service Bus if this setting is not specified within the transport. The minimum allowed value is 5 minutes. The transport will not apply this setting to topics or subscriptions as these are considered shared infrastructure (along with shared queues such as error and audit).

* `HierarchyNamespaceOptions`: Beginning version 6.1, this can be used to configure hierarchical entities. Azure Service Bus [allows entities to be organized in hierarchies on the same namespace](https://learn.microsoft.com/en-us/rest/api/servicebus/addressing-and-protocol). In a multi-tenant environment, this can be used to group tenant related queues and topics. It can also be used to separate work done by different developers (in non-production environments), separate production and lower environments, or to create a temporary infrastructure for troubleshooting a bug.
  
  The `HierarchyNamespace` property is used to prefix entity paths in the format `{HierarchyNamespace}/{entity}`.

  In scenarios when a message must be sent outside of the hierarchy, designated message types or interfaces can defined and excluded from the hierarchy using the `ExcludeMessageType<TMessageType>()` method:

  ```csharp
  // snippet placeholder
  public class MySingleExcludedMessage {}

  public interface IAmExcludedFromTheHierarchy {}

  public class MyExcludedMessageByInterface1 : IAmExcludedFromTheHierarchy {}
  ```

  ```csharp
  // snippet placeholder
  var transport = new AzureServiceBusTransport("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]", TopicTopology.Default);
  transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };

  // exclude only a concrete type
  transport.HierarchyNamespaceOptions.ExcludeMessageType<MySingleExcludedMessage>();

  // exclude all types that inherit an interface or base type
  transport.HierarchyNamespaceOptions.ExcludeMessageType<IAmExcludedFromTheHierarchy>();
  ```

> [!NOTE]
> The prefix is applied to all messages, so topology mappings do not need to specify the hierarchy namespace to utilize this feature. This also means that topology mappings are unable to "escape" the hierarchy if it is set. 
>
> If a topology mapping attempts to set the same hierarchy prefix, it will be ignored, but if it attempts to set a different one, it will still be prepended with the HierarchyNamespace. For example, if a topology mapping specifies a destination of topologyns/destination, the destination using the above options would become:
> ```
> my-hierarchy/topologyns/destination
> ```
>   For scenarios where this blanket hierarchy approach is not desired, there are two options:
> - Omit the HierarchyNamespace property and configure topology mappings for destinations where a hierarchy is desired. This is better when most messages will not be within the hierarchy.
> - Set the HierarchyNamespaceOptions property and configure message types that should be excluded using the `ExcludeMessageType<TMessageType>()` method.
>
> This is better when most messages need to be in the hierarchy.
