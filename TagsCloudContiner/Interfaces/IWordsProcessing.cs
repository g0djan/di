using System.Collections.Generic;

namespace TagsCloudContainer.Interfaces
{
    public interface IWordsProcessing
    {
        IEnumerable<string> WordsPreprocessing(IEnumerable<string> words);
    }
}
