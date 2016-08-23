
## Sharing data between Behaviors

Sometimes a parent behavior might need to pass some information to a child behavior and vice versa. The `context` parameter of a behavior's Invoke method facilitates passing data between behaviors. The context is very similar to a shared dictionary which allows adding and retrieving information from different behaviors.

snippet:SharingBehaviorData