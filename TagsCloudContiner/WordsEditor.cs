using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsEditor
    {
        IEnumerable<string> Edit(IEnumerable<string> words);
    }

    public class WordsEditor : IWordsEditor
    {
        public IEnumerable<string> Edit(IEnumerable<string> words)
        {
            return words;
        }
    }
}
