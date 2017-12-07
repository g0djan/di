using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace TagsCloudContainer
{
    public interface IWordsFilter
    {
        IEnumerable<string> FilterWords(IEnumerable<string> words);
        void AddBoringWords(IEnumerable<string> newBoringStrings);
    }

    public class WordsFilter : IWordsFilter
    {
        private readonly HashSet<string> boringWords;

        public WordsFilter()
        {
            var path = @"C:\Users\godja\OneDrive\Study\ШПоРа\di\TagsCloudContiner\Resources\stopwords.txt";
            boringWords = new HashSet<string>(File.ReadAllText(path).Split(' '));
        }

        public void AddBoringWords(IEnumerable<string> newBoringStrings) =>
            newBoringStrings.ToList().ForEach(boringString => boringWords.Add(boringString));

        public IEnumerable<string> FilterWords(IEnumerable<string> words) =>
            words.Select(word => word.ToLower())
                .Where(word => !boringWords.Contains(word))
                .ToArray();
    }
}