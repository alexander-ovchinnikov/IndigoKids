using System.Collections.Generic;
using Game.GUIElements;
using UnityEngine;

namespace Game
{
    public class InputController : MonoBehaviour
    {
        public delegate void OnKeyPressed(char letter);

        private readonly Dictionary<char, TextButton> _letterButtons = new Dictionary<char, TextButton>();
        [SerializeField] private TextButton _textButtonTemplate;

        public event OnKeyPressed OnKeyPressedEvent;

        private void Start()
        {
            Init();
        }


        public void OnNewLevel()
        {
            foreach (var letterButtonsValue in _letterButtons.Values) letterButtonsValue.Show();
        }

        private void Init()
        {
            var letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            foreach (var letter in letters) _letterButtons[letter] = CreateKeyBoardButton(letter);
        }

        private TextButton CreateKeyBoardButton(char letter)
        {
            return _textButtonTemplate.Duplicate(transform, letter.ToString(),
                delegate { OnButtonPress(letter); });
        }

        private void OnButtonPress(char letter)
        {
            _letterButtons[letter].Hide();
            if (OnKeyPressedEvent != null) OnKeyPressedEvent.Invoke(letter);
        }
    }
}