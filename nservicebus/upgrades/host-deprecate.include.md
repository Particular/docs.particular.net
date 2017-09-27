The following are the main reasons why the NServiceBus hosts are being deprecated:

- **Ease of configuration**: Endpoint configuration and related APIs have been significantly improved over the last few years. Therefore self-hosting is much simpler to set up than it used to be when hosts were first introduced.

- **Ease and transparency of deployment**: The hosts provide abstractions for service installations that configure most of the relevant settings, but still require performing additional manual steps to ensure reliability. The additional steps were sometimes accidentally skipped, causing issues after deployment. Now the installation process is more explicit and guides through service configuration in a more transparent manner.

- **Configuration transparency**: The hosts used to have various profiles and roles which automatically set certain options like disabling transactions. The effect was not obvious without reading the documentation, which could result in misunderstandings and undesired, surprising effects in the system at runtime. Self-hosting comes with full, explicit, fine-grained control over all aspects of the endpoint configuration and runtime behavior.

- **Easier troubleshooting**: The hosts introduce another layer of abstraction which can make troubleshooting more difficult. Some problems require deep dive into internal implementation and tend to be hard to resolve without support.

- **Better performance**: Using the hosts with default, generic settings introduces certain overhead. Self-hosting typically leads to shorter startup times and less memory consumption. With self-hosting, more control over the configuration is enabled.
