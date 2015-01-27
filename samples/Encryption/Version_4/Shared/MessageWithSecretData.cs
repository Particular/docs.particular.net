using System.Collections.Generic;
using NServiceBus;

public class MessageWithSecretData : IMessage
{
    public WireEncryptedString Secret { get; set; }
    public MySecretSubProperty SubProperty { get; set; }
    public List<CreditCardDetails> CreditCards { get; set; }
}