### Endpoint Docker image

Each endpoint is a container built on top of the official `mcr.microsoft.com/dotnet/runtime:7.0` image from [Docker Hub](https://hub.docker.com/). The container image builds and publishes the endpoint binaries and then uses those artifacts to build the final container image:

snippet: receiver
