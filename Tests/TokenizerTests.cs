using System;
using System.Linq;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twitterer.Core;

namespace Tests
{
    [TestClass]
    public class TokenizerTests
    {
        private Tokenizer SUT;

        [TestInitialize]
        public void Setup()
        {
            SUT = new Tokenizer();
        }

        [TestMethod]
        public void HasNoHashTagsNoMentions()
        {
            var document = new FlowDocument();
            var paragraph = new Paragraph(new Run("a b"));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void HashTag()
        {
            var document = new FlowDocument();
            var hashTag = "#a";
            var paragraph = new Paragraph(new Run(hashTag));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(hashTag, result.First().Text);
        }

        [TestMethod]
        public void HashHashTag()
        {
            var document = new FlowDocument();
            var hashTag = "##a";
            var paragraph = new Paragraph(new Run(hashTag));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(hashTag.Substring(1,2), result.First().Text);
        }

        [TestMethod]
        public void AtHashTag()
        {
            var document = new FlowDocument();
            var hashTag = "@#a";
            var paragraph = new Paragraph(new Run(hashTag));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(hashTag.Substring(1, 2), result.First().Text);
        }

        [TestMethod]
        public void Mention()
        {
            var document = new FlowDocument();
            var hashTag = "@a";
            var paragraph = new Paragraph(new Run(hashTag));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(hashTag, result.First().Text);
        }

        [TestMethod]
        public void MentionThenHashTag()
        {
            var document = new FlowDocument();
            var paragraph = new Paragraph(new Run("@a #b"));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void HashtagThenMention()
        {
            var document = new FlowDocument();
            var paragraph = new Paragraph(new Run("#a @b"));
            document.Blocks.Add(paragraph);

            var result = SUT.Tokenize(paragraph);
            Assert.AreEqual(2, result.Count());
        }
    }
}
