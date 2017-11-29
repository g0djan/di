using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHunspell;
using TagsCloudContiner.Interfaces;
using OpenNLP.Tools.Tokenize;

namespace TagsCloudContiner
{
    class TxtReader : IFileReader
    {
        public IEnumerable<string> ReadFile(string filename)
        {
            //var hunspell = new EnglishMaximumEntropyTokenizer();
            return File.ReadAllText(filename).Split();
        }
    }
}
