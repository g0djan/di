using System.Collections.Generic;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface IWordsEditor
    {
        Result<IEnumerable<string>> Edit(IEnumerable<string> words);
    }

    public class WordsEditor : IWordsEditor
    {
        public Result<IEnumerable<string>> Edit(IEnumerable<string> words) => Result.Ok(words);
    }
}
