If the trigger function must be customized, disable generation of the trigger function by removing the `NServiceBusTriggerFunction` attribute. A custom trigger function can then be added manually to the project:

snippet: custom-trigger-definition

Make sure the endpoint in `UseNServiceBus` matches the queue name in the function as illustrated above.