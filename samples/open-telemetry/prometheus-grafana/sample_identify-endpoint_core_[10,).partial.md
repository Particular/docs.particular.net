## Identifying the endpoint

An OpenTelemetry [resource](https://opentelemetry.io/docs/concepts/resources/) describes the process that produces telemetry, and it applies to all metrics, traces, and logs from that process. The sample sets the endpoint name as the `service.name` resource attribute, along with a unique `service.instance.id` for each run:

snippet: opentelemetry-resource

NServiceBus does not add the endpoint name as a tag on its metrics. The service name belongs to the resource, which the application sets when it configures the OpenTelemetry SDK. Adding an endpoint name tag would repeat the same value on every measurement. Metrics still carry the `nservicebus.queue` tag, which correlates each measurement with the queue that the endpoint receives from.

> [!NOTE]
> The resource is shared by everything in the process, so the service name identifies the endpoint only when the process hosts a single endpoint. When a process hosts multiple endpoints, use the `nservicebus.queue` tag to tell them apart.

When the endpoint runs as a project in [Aspire](/samples/hosting/aspire/), `AddProject` sets the `OTEL_SERVICE_NAME` environment variable to the project name. `ResourceBuilder.CreateDefault()` reads that variable, so the service name does not need to be set in code.
