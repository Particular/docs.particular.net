---
title: NServiceBus Mailer
component: Mailer
versions: '[3,)'
reviewed: 2016-12-21
related:
 - samples/mailer
---


## Enabling

snippet: MailerEnabling


## Usage


### In a Handler

snippet: MailerHandlerUsage


### In a Saga

snippet: MailerSagaUsage


## SmtpClient construction

By default the following [`SmtpClient`](https://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.aspx) construction is used the code

```cs
new SmtpClient
    {
        EnableSsl = true
    };
```

This results in the `SmtpClient` defaulting to reading its settings from the application config.

Configure custom `SmtpClient` construction using the following:

snippet: MailerSmtpBuilder


### Test/Development SmtpBuilder

For testing and development purposes it can be helpful to rout all email to a known directory:

snippet: MailerTestSmtpBuilder


## Attachments

Since it is not practical to send binary data as part of messages there is an alternative mechanism.

snippet: MailerAttachmentFinder


### Pass an `AttachmentContext` when sending the email

Pass an `AttachmentContext` when calling `SendMail`. The `AttachmentContext` should contain enough information to derive how to find and return the attachments for the email.

snippet: MailerPassAttachmentContext


## Error handling

Retrying email is difficult due to the fact that when sending an email to multiple addresses a subset of those addresses may return an error. In this case re-sending to all addresses would result in some addresses receiving the email multiple times.


### When all addresses fail

This case is when there is a generic exception talking to the mail server or the server returns an error that indicates all addresses have failed.

This is handling by letting the exception bubble to NServiceBus. This will result in falling back on the standard NServiceBus retry logic.


### When a subset of addresses fail

This will most likely occur when there is a subset of invalid addresses however there cases where the address can fail once and succeed after a retry. Have a look at [SmtpStatusCode](https://msdn.microsoft.com/en-us/library/system.net.mail.smtpstatuscode.aspx) for the possible error cases.

In this scenario it is not valid to retry the message since it would result in the message being resent to all recipients. It is also flawed to resend the verbatim email to the subset of failed addresses as this would effectively exclude them from some of the recipients in the conversation.

So the approach taken is to forward the original message to the failed recipients after prefixing the body with the following text

```
This message was forwarded due to the original email failing to send
-----Original Message-----
To: XXX
CC: XXX
Sent: XXX
```

While this is a little complex it achieves the desired of letting the failed recipients receive the email contents while also notifying them that there is a conversation happening with other recipients. It also avoids spamming the other recipients.