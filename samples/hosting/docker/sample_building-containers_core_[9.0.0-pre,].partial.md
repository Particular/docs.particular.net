### Building container images

Build the container images using `dotnet publish` which will `dotnet restore` and `dotnet build` the endpoints in addition to building the container images for both the `Sender` and the `Receiver`:

```bash
$ dotnet publish --os linux --arch x64 -c Debug /t:PublishContainer
```
