using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Twitterer.Core;

namespace HashtagHighlighter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            richTextBox.TextChanged -= richTextBox_TextChanged;
            var document = richTextBox.Document;
            var tokenizer = new Tokenizer();
            var tokens = e.Changes.SelectMany(x =>
            {
                var changePointer = document.ContentStart.GetPositionAtOffset(x.Offset);
                var paragraph = changePointer?.Paragraph;
                return paragraph == null ? new List<TextRange>() : tokenizer.Tokenize(paragraph);
            });

            foreach (var token in tokens)
            {
                token.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
            richTextBox.TextChanged += richTextBox_TextChanged;
        }
    }
}
