﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class WordsProvider 
    {
        public enum WordsFilterTypes
        {
            Unique,
            MoreFrequent,
            LessFrequent
        }

        private const int DEFAULT_WORD_LIMIT = 3;


        private readonly IEnumerator<string> _words;


        private WordsProvider(TextAsset asset, int wordLimit, WordsFilterTypes wordsFilterType)
        {
            var wordsQuery = asset.text.Split(' ')
                .Where(q => q.Length >= wordLimit)
                .GroupBy(q => q.ToLower())
                .Where(g => g.Key.Length >= wordLimit);
            switch (wordsFilterType)
            {
                case WordsFilterTypes.Unique:
                    wordsQuery = wordsQuery.Where(g => g.Count() == 1);
                    break;
                case WordsFilterTypes.LessFrequent:
                    wordsQuery = wordsQuery.OrderByDescending(q => q.Count());
                    break;
                case WordsFilterTypes.MoreFrequent:
                    wordsQuery = wordsQuery.OrderBy(q => q.Count());
                    break;
                default:
                    throw new ArgumentOutOfRangeException("wordsFilterType", wordsFilterType, null);
            }


            _words = wordsQuery.Select(g => g.Key).ToList().GetEnumerator();
        }


        public string GetNextWord()
        {
            _words.MoveNext();
            var word = _words.Current;
            return word;
        }


        public void Reset()
        {
            _words.Reset();
        }

        public static WordsProvider Init(TextAsset asset, int wordLimit = DEFAULT_WORD_LIMIT,
            WordsFilterTypes wordsFilterType = WordsFilterTypes.Unique)
        {
            return new WordsProvider(asset, wordLimit, wordsFilterType);
        }
    }
}