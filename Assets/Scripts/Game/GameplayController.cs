using System;
using System.Collections.Generic;
using Game.GUIElements;
using UnityEngine;

namespace Game
{
    public class GameplayController : MonoBehaviour
    {
        private readonly Dictionary<char, List<TextItem>> _textItemDictionary = new Dictionary<char, List<TextItem>>();
        private readonly List<TextItem> _textItems = new List<TextItem>();
        [SerializeField] private TextItem _textItemPrefab;
        public event Action OnWordComplete;
        public event Action OnLetterMiss;

        public void OnLetterInput(char l)
        {
            var letter = char.ToLower(l);
            if (_textItemDictionary.ContainsKey(letter))
            {
                _textItemDictionary[letter].ForEach(letterItem => { letterItem.Show(); });
                _textItemDictionary.Remove(letter);
                if (_textItemDictionary.Count != 0) return;
                if (OnWordComplete != null) OnWordComplete.Invoke();
            }
            else
            {
                if (OnLetterMiss != null) OnLetterMiss.Invoke();
            }
        }

        public void Init(string word)
        {

            _textItems.ForEach(x =>
            {
                {
                    x.gameObject.SetActive(false);
                    Destroy(x.gameObject);
                }
            });
            _textItems.Clear();
            _textItemDictionary.Clear();

            var letters = word.ToCharArray();
            foreach (var l in letters)
            {
                var letter = char.ToLower(l);
                var letterItem = _textItemPrefab.Duplicate(transform, letter.ToString());

                if (!_textItemDictionary.ContainsKey(letter)) _textItemDictionary[letter] = new List<TextItem>();


                _textItemDictionary[letter].Add(letterItem);
                _textItems.Add(letterItem);
            }
        }
    }
}