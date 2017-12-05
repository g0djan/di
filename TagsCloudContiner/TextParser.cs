using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public interface ITextParser
    {
        IEnumerable<string> GetWords(string text);
    }

    public class TextParser : ITextParser
    {
        public IEnumerable<string> GetWords(string text)
        {
            var punctuation = text.Where(char.IsPunctuation).Distinct().ToArray();
            return text.Split().Select(x => x.Trim(punctuation));
        }
    }
}
