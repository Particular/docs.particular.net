Command routing can be configured in code. The routing API is attached to the transport configuration object because some routing APIs are transport-specific. The routes can be specified at assembly, namespace, or specific type levels.

snippet: Routing-Logical

The routing engine prevents ambiguous routes, so if route information comes from more than one source (e.g., code API, and configuration file), the user must ensure that the type specifications do not overlap. If they do, an exception will be thrown, preventing the endpoint from starting up.
