namespace Store.Messages.Commands
{
    using NServiceBus;
    using WireEncryptedString = NServiceBus.Encryption.MessageProperty.WireEncryptedString;

    public class SubmitOrder :
        ICommand
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
        public WireEncryptedString CreditCardNumber { get; set; }
        public WireEncryptedString ExpirationDate { get; set; }
    }
}