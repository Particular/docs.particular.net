using System;
using System.Collections.Generic;
using NServiceBus;
using Newtonsoft.Json.Encryption;

#region Message

public class MessageWithSecretData :
    IMessage
{
    [Encrypt]
    public string Secret { get; set; }
    public MySecretSubProperty SubProperty { get; set; }
    public List<CreditCardDetails> CreditCards { get; set; }
}

public class MySecretSubProperty
{
    [Encrypt]
    public string Secret { get; set; }
}

public class CreditCardDetails
{
    public DateTime ValidTo { get; set; }
    [Encrypt]
    public string Number { get; set; }
}

#endregion