## Introduction to Consumer Driven Contracts
This sample shows a [consumer driven contracts](http://martinfowler.com/articles/consumerDrivenContracts.html)(CDD) approach to messaging. The essense of consumer driven contracts is that the owner ship of the contract is inversed. Instead of the producer defining the contract consumers are now the ones defining the contract they expect and it's up to the producer to fullfill it.

In the NServiceBus world message contracts is plain C# types so to honor a consumer contract the producer would just make the relevant message contract inherit from the contract type. 

### Contracts as interfaces

CDD is the main reason that `interface` messages are supported in addition to classes and the reason is that more than one consumer can provide a contract that would be satisfied by the same publisher message. If only classes where used this would not be possible since multiple inheritance isn't supported by C#. The solution is to use interfaces instead since with interfaces the publisher could implement all relevant contract types on the same message type.

Assuming that the producers `MyEvent` would satisfy both Consumer1Contract and Consumer2Contract this would look like this:

snippet: publisher-contracts

NOTE: The limitation of this approach is two or more consumers requiring a property with the same name but different types.

## Running the sample

Hit F5 to run the sample and notice how both consumers recives its contract when the producer publishes the `MyEvent`.

NOTE: Sharing contract types between enpoints is a larger topic and this sample is using linked files for simplicity. See the [message contacts documentation](/nservicebus/messaging/evolving-contracts.md) for more details. 