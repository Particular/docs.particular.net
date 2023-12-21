### Building container images

Building the container images using the following command will `dotnet publish` (which includes `dotnet restore` and `dotnet build`) the endpoints in addition to building the container images for both the `Sender` and the `Receiver`:

```bash
$ docker-compose build
```
