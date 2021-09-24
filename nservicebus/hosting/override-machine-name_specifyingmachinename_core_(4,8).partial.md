To override the machine name resolution, provide a factory method to `NServiceBus.Support.RuntimeEnvironment.MachineNameAction` when an endpoint is configured.

INFO: Override the `MachineNameAction` **before** creating an NServiceBus endpoint configuration object. Otherwise, messages could be sent that will not contain the right machine name values.

snippet: MachineNameActionOverride