using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    class TextFilter : IWordsProcessing
    {
        private readonly HashSet<string> boringWords;
        private readonly HashSet<Func<string, bool>> conditions;
        public Func<string, string> modificator { private get; set; }

        public TextFilter()
        {
            boringWords = new HashSet<string>(new WebClient()
                .DownloadString(@"https://jeroen.github.io/files/stopwords.txt")
                .Split(' ')); ;
            conditions = new HashSet<Func<string, bool>>();
            modificator = word => word;
        }
        
        public void AddBoringWords(params string[] newBoringStrings) => 
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public void RemoveBoringWords(params string[] boringStrings) => 
            boringStrings.ToList().ForEach(boringString => boringWords.Remove(boringString));

        public void AddConditionForWords(Func<string, bool> condition) => conditions.Add(condition);

        public void RemoveConditionForWords(Func<string, bool> condition) => conditions.Remove(condition);

        public IEnumerable<string> WordsPreprocessing(IEnumerable<string> words) => 
            words.Select(word => word.ToLower())
            .Where(word => !boringWords.Contains(word) && conditions.All(condition => condition(word)))
            .Select(word => modificator(word))
            .ToArray();
    }
}
