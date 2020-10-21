using System;
using System.Collections.Generic;

#region Message

// Note all the properties to encrypt start with "Encrypted"
public class MessageWithSecretData
{
    public string EncryptedSecret { get; set; }
    public MySecretSubProperty SubProperty { get; set; }
    public List<CreditCardDetails> CreditCards { get; set; }
}

public class MySecretSubProperty
{
    public string EncryptedSecret { get; set; }
}

public class CreditCardDetails
{
    public DateTime ValidTo { get; set; }
    public string EncryptedNumber { get; set; }
}

#endregion