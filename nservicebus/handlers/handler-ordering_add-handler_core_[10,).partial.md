This API is primarily intended to be used for [manually registering handlers](/nservicebus/handlers-and-sagas-registration.md#manual-registration). Handlers are invoked in the order they are registered.

> [!NOTE]
When handler registration via [assembly scanning](/nservicebus/handlers-and-sagas-registration.md#assembly-scanning) is used along with manual registration, the manually registered handlers will be executed in order before any handlers discovered via assembly scanning.