using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Game.Interfaces;
using UnityEngine;

namespace Game
{
    public class WordsProvider : IWordsProvider
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
            //_minWordLeangth = wordLimit;
            //Debug.Log(wordLimit);
            //Regex re = new Regex("[^A-z]|\b.{}{0," + wordLimit + ",}\b");

            //string filtered = re.Replace(asset.text.ToLower(), " ");
            //var wordsQuery = filtered.Split(' ').GroupBy(w => w);
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
            Debug.Log(">>" + word + "<<");
            return word;
        }


        public void Reset()
        {
            _words.Reset();
        }
        //private int? _minWordLeangth = null;

        public static IWordsProvider Init(TextAsset asset, int wordLimit = DEFAULT_WORD_LIMIT,
            WordsFilterTypes wordsFilterType = WordsFilterTypes.Unique)
        {
            return new WordsProvider(asset, wordLimit, wordsFilterType);
        }
    }


    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int _minWordCount;
        [SerializeField] private TextAsset _source;
        [SerializeField] private int _startAttempts;

        [SerializeField]
        private WordsProvider.WordsFilterTypes _wordsFilterType = WordsProvider.WordsFilterTypes.Unique;

        public int StartAttempts
        {
            get { return _startAttempts; }
        }

        public IWordsProvider GetWordsProvider()
        {
            return WordsProvider.Init(_source, _minWordCount, _wordsFilterType);
        }
    }
}