If the trigger function must be customized, you must disable generation of the trigger function by removing the `NServiceBusTriggerFunction` attribute. A custom trigger function can then be added manually to the project:

snippet: custom-trigger-definition

The endpoint name in `UseNServiceBus` must match the queue name in the function, as illustrated above.
