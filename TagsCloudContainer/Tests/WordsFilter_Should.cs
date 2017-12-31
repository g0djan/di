using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer
{
    [TestFixture]
    public class WordsFilter_Should
    {
        [Test]
        public void AddWords_RemoveItFromResult()
        {
            var words = new[] { "kek", "aviasales", "richbich" };
            var filter = new WordsFilter();
            filter.AddBoringWords(words.Take(2));
            filter.FilterWords(words).GetValueOrThrow().ToArray().Should().BeEquivalentTo(words[2]);
        }
    }
}