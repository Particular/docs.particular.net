
The MSMQ Transport uses the local machine name, taken from `Environment.MachineName`, as the originator and reply-to address in messages by default.

Some deployment environments require the use of [Full Qualified Domain Names (FQDM)](https://en.wikipedia.org/wiki/Fully_qualified_domain_name) to route messages correctly. The most common scenario is routing between machines in different domains.

To override the machine name resolution, provide a factory method to `RuntimeEnvironment.MachineNameAction` when an endpoint is configured.

Check [how to find FQDM of local machine](https://stackoverflow.com/questions/804700/how-to-find-fqdn-of-local-machine-in-c-net) for a good starting point on how to get the FQDM of the local machine.

snippet: MsmqMachineNameFQDN

The [routing](/nservicebus/messaging/routing.md) configuration and any explicit queue destinations will also need to use FQDM.