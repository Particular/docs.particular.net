---
title: CloudEvents
summary: Provides details about CloudEvents support in NServiceBus and the platform
component: Core
reviewed: 2026-02-05
versions: '[10.1,]'
related:
- samples/aws/cloud-events
- samples/azure-service-bus-netstandard/cloud-events
---

> [!WARNING]
> This is an experimental feature and, as such, is subject to changes.

This guideline explains how to configure NServiceBus endpoints to receive [CloudEvents](https://cloudevents.io/).

> [!NOTE]
> How are you using CloudEvents? [Share your feedback](https://survey.alchemer.com/s3/8658927/CloudEvents-Survey) about how CloudEvents fit into your development life cycle.

## Supported formats

This section describes [CloudEvents formats](https://github.com/cloudevents/spec/tree/v1.0.2/cloudevents/formats) supported by NServiceBus.

### JSON

NServiceBus supports the [JSON format](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/formats/json-format.md#3-envelope). The [JSON Batch format](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/formats/json-format.md#4-json-batch-format) is not supported.

## Supported bindings

This section describes [CloudEvents bindings](https://github.com/cloudevents/spec/tree/v1.0.2/cloudevents/bindings) supported by NServiceBus.

### Structured Content Mode for HTTP and AMQP

NServiceBus supports [HTTP Structured](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/http-protocol-binding.md#32-structured-content-mode) and [AMQP Structured](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/amqp-protocol-binding.md#32-structured-content-mode) content modes. For each content mode, two implementations are provided:

- Strict
- Permissive

#### Strict mode

In Strict mode, the CloudEvents message `Content-Type` header must be equal to or contain `application/cloudevents+json`. The implementation will not attempt to deserialize the payload if the `Content-Type` header does not meet the requirements.

Fields [`id`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#id), [`source`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#source-1), [`type`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#type), and at least one of `data` or `data_base64` must be present. The [`specversion`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#specversion) is not required to be present or to equal to `1.0`.

#### Permissive mode

In Permissive mode, the CloudEvents message [`type`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#type) field must be present. The `Content-Type` header is not verified, and a payload deserialization attempt is always executed.

### Binary Content Mode for HTTP and AMQP

NServiceBus supports the [HTTP Binary](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/http-protocol-binding.md#31-binary-content-mode) and [AMQP Binary](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/amqp-protocol-binding.md#31-binary-content-mode) content modes.

To recognize the CloudEvents message, the fields [`id`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#id), [`source`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#source-1), and [`type`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#type) must be present. The [`specversion`](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#specversion) field is not required to be present or to equal to `1.0`.

Field names must be encoded according to the binding specification (e.g. `ce-id` for the `id` field when using HTTP).

The implementation will not attempt to deserialize the payload if the headers do not meet the requirements.

The [HTTP Content-Type](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/http-protocol-binding.md#311-http-content-type) and [AMQP Content-Type](https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/bindings/amqp-protocol-binding.md#321-amqp-content-type) headers are not validated.

## Configuration

This section describes the configuration options.

### Enabling CloudEvents

To enable the CloudEvents support:

snippet: cloudevents-configuration

### Type mapping

`TypeMappings` configure how to match the incoming message content-type value with the class definition used in the NServiceBus message handler:

snippet: cloudevents-typemapping

### Enabled unwrappers

The `EnvelopeUnwrappers` property contains the list of enabled modes. By default, both Binary Content Mode and Structured Content Mode in Strict Mode are enabled.

To enable or disable unwrappers, modify the `EnvelopeUnwrappers` property by adding or removing unwrappers.

The following code snippet shows how to change the JSON Structured Content unwrapper from Strict to Permissive:

snippet: cloudevents-json-permissive

## Metrics and logging

The package provides metrics and logs insights into every stage of receiving a message.

When selecting an unwrapper for the message:
- A warning message is logged when the unwrapper fails to unwrap the message.
- A metric is emitted for every unwrapping attempt. The metric's value indicates whether the unwrapping succeeded or crashed. Keep in mind that, if the unwrapper recognizes that it can't unwrap the message (e.g. because the message lacks required fields), then it's considered a successful unwrapping.

Structured Content Mode in Strict Mode produces the following signals:
- If the message has the correct content type header, a metric is emitted.
- If, and only if, the message has the correct content type header, a metric is emitted and a warning message is logged if the message's body cannot be deserialized, or if the message doesn't have at least one required field.
- If, and only if, the message has the correct content type header and all required fields, a metric is emitted and a warning message is logged if the version field contains an unexpected value, or if the version field is missing.

Structured Content Mode in Permissive Mode produces the following signals:
- A metric is emitted to indicate that the unwrapper attempts to unwrap the message (i.e. for every message).
- If, and only if, the message could be parsed, and the message contains the type field, a metric is emitted and a warning message is logged if the version field contains an unexpected value.

Binary Content Mode produces the following signals:
- A metric is emitted if at least one of the required headers is absent.
- A metric is emitted if the required headers are present.
- If, and only if, the message has required headers, a metric is emitted and a warning message is logged if the version header contains an unexpected value, or if the version header is missing

## Troubleshooting

To troubleshoot the mechanism, examine the metrics and logs to understand why messages are not unwrapped as CloudEvents. A non-exhaustive list of elements to check includes:

- Is the mechanism enabled?
- Are correct unwrappers registered?
- Is the mode for Structured Content Mode messages configured correctly?
- Is the JSON deserializer configured to handle lowercase/uppercase letters correctly?
- Do the incoming messages have all required headers and fields? Pay special attention to lowercase/uppercase letters.
- Is the type mapping configured correctly?
