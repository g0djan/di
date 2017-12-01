using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class TextParser
    {
        public IEnumerable<string> GetWords(string text)
        {
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            return text.Split().Select(x => x.Trim(punctuation));
        }
    }
}
