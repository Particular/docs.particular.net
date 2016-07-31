## Performance Impact

WARNING: Notifications are executed on the same thread as the NServiceBus pipeline. If long running work needs to be done it should be executed on another thread otherwise the message processing performance can be impacted.