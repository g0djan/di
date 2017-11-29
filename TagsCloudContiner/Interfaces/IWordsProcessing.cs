using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContiner.Interfaces
{
    public interface IWordsProcessing
    {
        IEnumerable<string> WordsPreprocessing(IEnumerable<string> words);
    }
}
