using System.Collections.Generic;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface ITextParser
    {
        Result<IEnumerable<string>> GetWords(string text);
    }

    public class PunctuationParser : ITextParser
    {
        public Result<IEnumerable<string>> GetWords(string text)
        {
            return Result.Of(() =>
            {
                var punctuation = text.Where(char.IsPunctuation).Distinct().ToArray();
                return text.Split().Select(x => x.Trim(punctuation));
            });
        }
    }
}
