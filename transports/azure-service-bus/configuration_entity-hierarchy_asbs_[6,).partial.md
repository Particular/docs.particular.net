### Hierarchy namespace

 Azure Service Bus [allows entities to be organized in hierarchies on the same namespace](https://learn.microsoft.com/en-us/rest/api/servicebus/addressing-and-protocol). In a multi-tenant environment, this can be used to group tenant related queues and topics. It can also be used to separate work done by different developers (in non-production environments), separate production and lower environments, or to create a temporary infrastructure for troubleshooting a bug.

From version 6.1 onward, the Azure Service Bus transport supports configuring hierarchical entities by setting the `HierarchyNamespaceOptions` with a `HierarchyNamespace`.  Doing so will prefix all entities with the `{HierarchyNamespace}/{original-entity-path}` format.

#### Escaping the hierarchy

 In scenarios when a message must be sent outside of the hierarchy, designated message types or interfaces can defined and excluded from the hierarchy using the `HierarchyNamespaceOptions.ExcludeMessageType<TMessageType>()` method:

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

#### Usage with topology mapping

The hierarchy namespace prefix is applied to all messages, so topology mappings do not need to specify the hierarchy namespace to utilize this feature. This also means that topology mappings are unable to "escape" the hierarchy if it is set. 

If a topology mapping attempts to set the same hierarchy prefix, it will be ignored, but if it attempts to set a different one, it will still be prepended with the hierarchy namespace.

For example, if a topology mapping specifies a destination of topology-ns/destination, the destination using the above options would become:
```
my-hierarchy/topology-ns/destination
```
For scenarios where this blanket hierarchy approach is not desired, there are two options:
- Do not set the `HierarchyNamespaceOptions` property and configure topology mappings for destinations where a hierarchy is desired. This is better when most messages will not be within the hierarchy.
- Set the `HierarchyNamespaceOptions` property and configure message types that should be excluded using the `ExcludeMessageType<TMessageType>()` method.

This is better when most messages need to be in the hierarchy.
