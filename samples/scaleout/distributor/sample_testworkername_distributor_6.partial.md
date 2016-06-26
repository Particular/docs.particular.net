#### Version 6

The worker input queue is generated in the same way as for a non-scaled out endpoint -- based on the endpoint name. If multiple workers are deployed to same machine, they share the input queue. Because in this sample two workers need to run to show the actual distribution, the input queue cannot be shared. In order to prevent it an address translation exception is used.

snippet:AddressTranslationExecption