using System;
using System.Collections.Generic;
using NServiceBus;
using Newtonsoft.Json.Encryption;

#region Message

public class MessageWithSecretData :
    IMessage
{
    [Encrypt]
    public string Secret;
    public MySecretSubProperty SubProperty;
    public List<CreditCardDetails> CreditCards;
}

public class MySecretSubProperty
{
    [Encrypt]
    public string Secret;
}

public class CreditCardDetails
{
    public DateTime ValidTo;
    [Encrypt]
    public string Number;
}

#endregion