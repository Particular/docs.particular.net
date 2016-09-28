### OverrideBinding

It is also possible to configure the binding and address in code for each service type individually:

snippet:WcfOverrideBinding

The delegate is invoked for each service type discovered. The delegate needs to return a binding configuration which contains the binding instance to be used as well as optionally an absolute listening address.