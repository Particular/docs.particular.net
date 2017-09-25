The following are the main reasons why we are deprecating the NServiceBus Hosts:

- **Ease of configuration**: Endpoint configuration and related APIs have been significantly improved over the last few years. Therefore self-hosting is much simpler to set up than it used to be when Host was first introduced.

- **Ease and transparency of deployment**: The Host provided abstractions for service installations that configured most of the relevant settings, but still required performing additional manual steps to ensure reliability. The additional steps were sometimes skipped, causing issues after deployment. The installation process is now more explicit and guides users to configure services in a more transparent manner.

- **Configuration transparency**: The Host used to have various profiles and roles which automatically set certain options like disabling transactions. That was not obvious without reading the documentation. It could then cause undesired, surprising effects in the system at runtime. With self-hosting users have the full, explicit, fine-grained control over all aspects of the endpoint configuration and runtime behavior.

- **Easier troubleshooting**: The Host introduces another layer of abstraction which can make troubleshooting more difficult. Some problems required deep dive into internal implementation and were hard to solve without our support.

- **Better performance**: Using the Host with default, generic settings introduces certain overhead. Users using self-hosting typically observe shorter startup times and less memory consumption.
