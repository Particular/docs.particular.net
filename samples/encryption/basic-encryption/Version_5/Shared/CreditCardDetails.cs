using System;
using NServiceBus;

public class CreditCardDetails
{
    public DateTime ValidTo { get; set; }
    public WireEncryptedString Number { get; set; }
}