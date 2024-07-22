## Monitoring

### Heartbeats

Version 2.2 of the NServiceBus Messaging Bridge adds support for the heartbeats feature. This feature sends regular heartbeat messages from each messaging bridge transport to a ServiceControl instance. The ServiceControl instance keeps track of which endpoint instances are sending heartbeats and which ones are not.  See the [Hearbeats plugin documentation](/monitoring/heartbeats/) for more information.

Heartbeats can be configured on a per bridge transport basis.  For each bridge transport that should send hearbeats, use the `.SendHeartbeatTo` as follows:

snippet: configure-heartbeats

> [!NOTE]
> `ServiceControl_Queue` is a placeholder for the name of the ServiceControl input queue. The name of the ServiceControl input queue matches the [ServiceControl instance name](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolinstancename).

#### Heartbeat interval

Heartbeat messages are sent at a default frequency of 10 seconds. As shown above, the frequency may be overridden for each endpoint.

> [!NOTE]
> The frequency must be lower than the [`HeartbeatGracePeriod`](/servicecontrol/servicecontrol-instances/configuration.md#plugin-specific-servicecontrolheartbeatgraceperiod) in ServiceControl.

#### Time-To-Live (TTL)

Heartbeat messages are sent with a default TTL of four times the frequency. As shown above, the TTL may be overridden for each endpoint. See the documentation for [expired heartbeats](/monitoring/heartbeats/expired-heartbeats.md) for more information.

#### Identifying scaled-out endpoints

When heartbeats are being used in a [scaled-out Messaging Bridge that is using competing consumers](/nservicebus/bridge/performance.md#scaling-out-competing-consumers), each bridge transport must be configured with a unique name (see [Configuring transports](#auditing-configuring-transports)). The bridge transport name is used in the heartbeat messages so ServiceControl can track of all instances and identify which instance sent a given heartbeat message.

### Custom checks

Version 2.2 of the Messaging Bridge adds support for custom checks.  The custom checks feature enables Messaging Bridge health monitoring by running custom code and reporting status (success or failure) to a ServiceControl instance. See the [custom checks plugin documentation](/monitoring/custom-checks/) for more information.  For documentation on how to write custom checks, see the [Writing Custom Checks document](/monitoring/custom-checks/writing-custom-checks.md)

The custom checks feature can be enabled per bridge transport by using `ReportCustomChecksTo` as follow:

snippet: configure-custom-checks

> [!NOTE]
> Each custom check is executed once, and the result is sent to each bridge transport configured to report custom checks. When configured to [bridge platform queues](#bridging-platform-queues), it is only necessary to report custom checks on one of the bridge transports. Otherwise, a custom check result will be reported multiple times to the same platform instance.

#### Time-To-Live (TTL)

Custom check results are sent with a default TTL of four times the interval for periodic checks or infinite for one-time checks. As shown above, the TTL may be overridden for each bridge transport.
