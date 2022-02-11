## Custom audit action

The example below shows how to extend the pipeline with a behavior that:

- Stores the message body in an external storage
- Excludes the body from the message sent to the audit queue
- Adds a metadata entry that links to the stored body

snippet: custom-audit-action

In addition, the behavior needs to be [registered in the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md#add-a-new-step).
