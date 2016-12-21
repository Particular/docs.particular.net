---
title: NServiceBus Mailer
reviewed: 2016-12-21
component: Mailer
---

## Introduction

Uses the [NServiceBus Mailer](/nservicebus/mailer/) project to send an email from both a saga and a handler.


## Code Walk-through


### Enable the Mailer feature

snippet: EnableMailer


### Configure the SmtpBuilder

This sample is hard coded to write all outgoing email to the filesystem at `Path.Combine(Environment.CurrentDirectory, "Emails")`.

snippet: smtpBuilder


### Attachment logic

In this sample a fake attachment is added and no cleanup is necessary

snippet: attachmentfinder


### The Handler

snippet: handler


### The Saga

snippet: saga


## Running the Sample

When the solution is run 'enter' can be presses to send an initiating message. This message will be received by both the handler and the saga. Both handler and saga will send an email message which will be written to `/bin/debug/emails/[GUID].eml`.