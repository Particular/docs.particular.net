## NServiceBus Docker Container

This template makes it easier to create a [Docker](https://www.docker.com/) container host for an NServiceBus endpoint. The template follows the approach outlined in [Docker Container Host](/nservicebus/hosting/docker-host/).

The template can be used via the following command:

snippet: docker-usage

This will create a new directory named `MyEndpoint` containing a `.csproj` also named `MyEndpoint`.

To add to an existing solution:

snippet: docker-addToSolution

To build the Docker image, open the command line, change to the directory containing the endpoint's `.csproj`, and run the following command:

snippet: docker-build