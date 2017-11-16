## Mutators versus Behaviors


### Shared concepts and functionality

Both mutators and behaviors:

 * Can manipulate pipeline state
 * Can be executed in the incoming or outgoing pipeline
 * Bubble exceptions up the pipeline and handle them by the [recoverability](/nservicebus/recoverability/) mechanism


### Differences

Note that these are relative differences. So, for example, a behavior is only "high complexity" in comparison to a mutator.

|                                   | Mutator | Behavior |
|-----------------------------------|---------|----------|
| Complexity to implement           | Low     | High     |
| Flexibility                       | Low     | High     |
| Location in pipeline              | Fixed   | Flexible |
| Complexity to test                | Low     | Medium*  |
| Can control nested action         | No      | Yes      |
| Affects call stack depth          | No      | Yes      |
| Can replace an existing behavior  | No      | Yes      |

* For more information refer to the [testing behaviors](/samples/unit-testing/) unit testing sample.
