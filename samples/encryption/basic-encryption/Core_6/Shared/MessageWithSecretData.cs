using System.Collections.Generic;
using NServiceBus;

#region Message
public class MessageWithSecretData :
    IMessage
{
    public WireEncryptedString Secret { get; set; }
    public MySecretSubProperty SubProperty { get; set; }
    public List<CreditCardDetails> CreditCards { get; set; }
}
#endregion