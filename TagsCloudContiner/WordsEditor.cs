using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public interface IWordsEditor
    {
        IEnumerable<string> EditWords(IEnumerable<string> words);
        void AddBoringWords(IEnumerable<string> newBoringStrings);
    }

    public class WordsEditor : IWordsEditor
    {
        private readonly HashSet<string> boringWords;
        private readonly Func<string, bool> partSpeechCondition;
        private Func<string, string> WordEditor { get; }

        public WordsEditor(Func<string, bool> partSpeechCondition, Func<string, string> wordEditor)
        {
            var path = string.Join(
                Path.DirectorySeparatorChar.ToString(),
                "..", "..", "Resources", "stopwords.txt");
            boringWords = new HashSet<string>(File.ReadAllText(path).Split(' '));
            this.partSpeechCondition = partSpeechCondition;
            WordEditor = wordEditor;
        }

        public void AddBoringWords(IEnumerable<string> newBoringStrings) =>
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public IEnumerable<string> EditWords(IEnumerable<string> words) =>
            words.Select(word => word.ToLower())
                .Where(word => !boringWords.Contains(word) && partSpeechCondition(word))
                .Select(WordEditor)
                .ToArray();
    }
}