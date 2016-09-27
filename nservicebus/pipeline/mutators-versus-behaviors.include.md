## Mutators versus Behavior


### Shared concepts and functionality

 * Can manipulate pipeline state
 * Can be executed in the incoming or outgoing pipeline
 * Exceptions cause bubble up the pipeline and are handled by the [Recoverability](/nservicebus/recoverability/)


### Differences

Note that these are relative differences. So, for example, a Behavior is only "high complexity" in comparison to a Mutator.

|                                | Mutator | Behavior |
|--------------------------------|---------|----------|
| Complexity to implement        | Low     | High     |
| Flexibility                    | Low     | High     |
| Location in pipeline           | Fixed   | Flexible |
| Complexity to test             | Low     | High     |
| Can control nested action      | No      | Yes      |
| Effects call stack depth       | No      | Yes      |
| Can replace existing Behavior  | No      | Yes      |