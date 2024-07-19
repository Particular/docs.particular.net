## Alignment of host identifier

It is recommended to align the instance identifier between NServiceBus and OpenTelemetry so all logs, metrics, traces and audit messages can be correlated by a host (instance) if needed.

> [!NOTE]
> The OpenTelemetry specification recommends this to be a random uuid. However, it may also be a [deterministic uuid v5](https://opentelemetry.io/docs/specs/semconv/attributes-registry/service/#service-attributes) (i.e. hash of machine name and  endpointname).

NServiceBus adds a [host identifier to all audit messages](/nservicebus/hosting/override-hostid.md) and this instance identifier is also used to show [performance metrics for each running instance in ServicePulse](/monitoring/metrics/in-servicepulse.md). The [instance identifier used for ServicePulse value can be overriden](/monitoring/metrics/install-plugin.md#configuration-instance-id).

OpenTelemetry also allows to customize the instance id used for `service.instance.id` in various ways. 

Consider aligning the instance ID used by OpenTelemetry and ServiceControl metrics API.

#### Example

snippet: opentelemetry-align-host-id
