using System.Windows.Media;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation;

namespace ServiceInsight.CustomViewer.Plugin
{
    public partial class MyCustomDecryptionView
    {
        readonly FoldingManager foldingManager;
        readonly XmlFoldingStrategy foldingStrategy;

        public MyCustomDecryptionView()
        {
            InitializeComponent();
            foldingManager = FoldingManager.Install(document.TextArea);
            foldingStrategy = new XmlFoldingStrategy();
            SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
            document.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
        }
        
        public void Clear()
        {
            document.Document.Text = string.Empty;
            foldingStrategy.UpdateFoldings(foldingManager, document.Document);
        }

        public void Show(string messageBody)
        {
            document.Document.Text = messageBody;
            foldingStrategy.UpdateFoldings(foldingManager, document.Document);
        }
    }
}
