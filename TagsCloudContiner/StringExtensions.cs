using System.Collections.Generic;
using TagsCloudContainer;

namespace TagsCloudContiner
{
    public static class StringExtensions
    {
        public static IEnumerable<string> ParseTextWith(this string text, TextParser parser) => parser.GetWords(text);
    }
}
