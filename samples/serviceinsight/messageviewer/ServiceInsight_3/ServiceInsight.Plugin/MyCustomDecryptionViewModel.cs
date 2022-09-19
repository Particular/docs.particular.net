namespace ServiceInsight.CustomViewer.Plugin
{
    using System.Text;
    using Shared;
    using Caliburn.Micro;
    using ServiceInsight.MessageViewers;
    using ServiceInsight.Models;
    using ServiceInsight.ServiceControl;

    public class MyCustomDecryptionViewModel : Screen, ICustomMessageBodyViewer
    {
        readonly IMessageEncoder messageEncoder;
        MyCustomDecryptionView view;
        
        public MyCustomDecryptionViewModel(IMessageEncoder messageEncoder)
        {
            this.messageEncoder = messageEncoder;
            DisplayName = "Decryption Viewer";
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            this.view = (MyCustomDecryptionView)view;
        }

        #region MessageDisplay
        public void Display(StoredMessage selectedMessage)
        {
            if (selectedMessage?.Body?.Text != null)
            {
                var bytes = Encoding.Default.GetBytes(selectedMessage.Body.Text);
                var decryptedBytes = messageEncoder.Decrypt(bytes);
                var clearText = Encoding.Default.GetString(decryptedBytes);
                
                view?.Show(clearText);
            }
        }
        #endregion

        public void Clear()
        {
            view?.Clear();
        }
        
        public bool IsVisible(StoredMessage selectedMessage, PresentationHint presentationHint)
        {
            return selectedMessage != null;
        }
    }
}
