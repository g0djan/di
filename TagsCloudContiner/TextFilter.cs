using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class TextFilter
    {
        private readonly HashSet<string> boringWords;
        private readonly Func<string, bool> condition;
        private Func<string, string> modificator { get; }

        public TextFilter(Func<string, bool> condition, Func<string, string> modificator)
        {
            boringWords = new HashSet<string>(new WebClient()
                .DownloadString(@"https://jeroen.github.io/files/stopwords.txt")
                .Split(' '));
            this.condition = condition;
            this.modificator = modificator;
        }
        
        public void AddBoringWords(IEnumerable<string> newBoringStrings) => 
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public void RemoveBoringWords(IEnumerable<string> boringStrings) => 
            boringStrings.ToList().ForEach(boringString => boringWords.Remove(boringString));

        public IEnumerable<string> WordsPreprocessing(IEnumerable<string> words) => 
            words.Select(word => word.ToLower())
            .Where(word => !boringWords.Contains(word) && condition(word))
            .Select(word => modificator(word))
            .ToArray();
    }
}
