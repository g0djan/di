using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer
{
    public interface ITextParser
    {
        Result<IEnumerable<string>> GetWords(string text);
    }

    public class TextParser : ITextParser
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
