NServiceBus can be configured to use [Autofac](https://autofac.org/) for dependency injection.

## Default usage

snippet: Autofac

## Using an existing container

snippet: Autofac_Existing

WARNING: Although it is possible to update the container after passing it to NServiceBus using the `ContainerBuilder.Update` method, from Autofac 4.2.1 onwards that method [is marked as obsolete](https://github.com/autofac/Autofac/issues/811). It is recommended not to use this method to update the container after it has been passed to NServiceBus.
