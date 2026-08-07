Before running the container image normally, it must run in setup mode to create the required message queues and perform upgrade tasks.

The container image will run in setup mode by adding the `--setup` argument. For example:

```shell
# Using docker run
docker run --rm {OPTIONS} {IMAGE} --setup
```

Setup mode may require different settings, such as a different transport connection string with permissions to create queues.

After setup is complete, the container will exit, and the `--rm` (or equivalent) option can be used to automatically remove it.

The setup process should be repeated any time the container is [updated to a new version](#upgrading).

### Simplified setup

Instead of running `--setup` as a separate container, the setup and run operations can be combined using the `--setup-and-run` argument:

```shell
# Using docker run
docker run {OPTIONS} {IMAGE} --setup-and-run
```

The `--setup-and-run` argument runs the setup process when the container starts, after which the application runs normally. This simplifies deployment by removing the need for a separate init container in environments where the setup process does not need different settings.

Using `--setup-and-run` removes the need to repeat a setup process when the container is updated to a new version.
