Command routing can be configured in code. The routing API root is attached to the transport configuration object because some routing APIs are transport-specific. The routes can be specified on assembly, namespace or specific type level.

snippet: Routing-Logical

Routing engine prevents ambiguous routes so if route information comes from more than one source (e.g. code API and configuration file) user has to make sure the type specifications do not overlap. Otherwise an exception will be thrown preventing an endpoint from starting up.