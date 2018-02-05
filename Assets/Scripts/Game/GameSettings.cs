using UnityEngine;

namespace Game
{
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

        public WordsProvider GetWordsProvider()
        {
            return WordsProvider.Init(_source, _minWordCount, _wordsFilterType);
        }
    }
}