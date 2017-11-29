using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NHunspell;
using TagsCloudContiner.Interfaces;

namespace TagsCloudContiner
{
    class TextFilter : IWordsProcessing
    {
        private readonly HashSet<string> boringWords = new HashSet<string>(new WebClient()
            .DownloadString(@"https://jeroen.github.io/files/stopwords.txt")
            .Split(' '));

        public void AddBoringWords(params string[] newBoringStrings) => 
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public void RemoveBoringWords(params string[] boringStrings) => 
            boringStrings.ToList().ForEach(boringString => boringWords.Remove(boringString));

        public IEnumerable<string> WordsPreprocessing(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower()).Where(word => !boringWords.Contains(word)).ToArray();
        }
    }
}
