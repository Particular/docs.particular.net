namespace Store.Messages.Commands
{
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    public class SubmitOrder :
        ICommand
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
        public EncryptedString CreditCardNumber { get; set; }
        public EncryptedString ExpirationDate { get; set; }
    }
}