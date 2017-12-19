using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer
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
