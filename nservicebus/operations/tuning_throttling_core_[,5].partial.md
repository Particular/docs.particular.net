Define a maximum value for the number of messages per second that the endpoint will process at any given time. This will help avoiding the endpoint from overloading sensitive resources that it's using like web-services, databases, other endpoints etc. Some examples would include

 * An integration endpoint calling a web API, like `api.github.com`, that have restrictions on the number or requests per unit of time allowed.
 * A firewall that, for flood protection, limits the number of connections per second and IP address.

By default no throughput restrictions will be enforced.
