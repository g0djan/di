using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer
{
    public interface IWordsFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
        void AddBoringWords(IEnumerable<string> newBoringStrings);
    }

    public class WordsFilter : IWordsFilter
    {
        private readonly HashSet<string> boringWords;

        public WordsFilter()
        {
            var path = string.Join(Path.DirectorySeparatorChar.ToString(), Directory.GetCurrentDirectory(),
                "..", "..", "Resources", "stopwords.txt");
            boringWords = new HashSet<string>(File.ReadAllText(path).Split(' '));
        }

        public void AddBoringWords(IEnumerable<string> newBoringStrings) =>
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return Result.Of(() =>
                words.Select(word => word.ToLower()).Where(word => !boringWords.Contains(word)));
        }
    }
}