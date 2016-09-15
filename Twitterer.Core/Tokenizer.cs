using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using ToriatamaText;

namespace Twitterer.Core
{
    public class Tokenizer
    {
        public IEnumerable<TextRange> Tokenize(Paragraph paragraph)
        {
            var extractor = new Extractor();
            var paragraphRange = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
            paragraphRange.ApplyPropertyValue(TextElement.ForegroundProperty, paragraph.Foreground);
            var runs = paragraph.Inlines.OfType<Run>();
            return runs.SelectMany(x =>
            {
                var range = new TextRange(x.ContentStart, x.ContentEnd);
                var text = x.Text;
                return extractor.ExtractHashtags(text, false)
                    .Union(extractor.ExtractMentionedScreenNames(text))
                    .Select(info => 
                        new TextRange(range.Start.GetPositionAtOffset(info.StartIndex),
                            range.Start.GetPositionAtOffset(info.StartIndex + info.Length)));
            });
        }
    }
}