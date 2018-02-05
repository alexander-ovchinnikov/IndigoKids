using System;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private int _currentAttempts;
        [SerializeField] private GameplayController _game;
        [SerializeField] private GameInterface _gameInterface;
        [SerializeField] private InputController _input;
        [SerializeField] private GameSettings _settings;

        private event Action WinEvent;
        private event Action LoseEvent;
        private event Action<int> AttemptsCountChanged;
        private WordsProvider _wordProvider;

        private int CurrentAttempts
        {
            get { return _currentAttempts; }
            set
            {
                _currentAttempts = value;
                if (AttemptsCountChanged != null) AttemptsCountChanged(value);
                if (value >= 0) return;
                if (LoseEvent != null) LoseEvent();
            }
        }


        private void OnWin()
        {
            CurrentAttempts = _settings.StartAttempts;
            ResetGame();
        }

        private void ResetGame()
        {
            _wordProvider.Reset();
            InitNewLevel();
        }

        private void IncreaseAttempts()
        {
            CurrentAttempts += CurrentAttempts;
        }

        private void OnLevelComplete()
        {
            IncreaseAttempts();
            InitNewLevel();
        }

        private void InitNewLevel()
        {
            var nextWord = GetNextWord();
            if (nextWord == null)
            {
                if (WinEvent != null) WinEvent();
            }
            else
            {
                _game.Init(nextWord);
            }

            _input.OnNewLevel();
        }

        private void OnLose()
        {
            CurrentAttempts = 0;
            ResetGame();
        }

        private void OnLetterMiss()
        {
            --CurrentAttempts;
        }


        private string GetNextWord()
        {
            return _wordProvider.GetNextWord();
        }


        private void Start()
        {
            _wordProvider = _settings.GetWordsProvider();


            _input.OnKeyPressedEvent += _game.OnLetterInput;
            _game.OnWordComplete += OnLevelComplete;
            _game.OnLetterMiss += OnLetterMiss;

            WinEvent += OnWin;
            WinEvent += _gameInterface.OnWin;

            LoseEvent += OnLose;
            LoseEvent += _gameInterface.OnLose;

            AttemptsCountChanged += _gameInterface.OnAttemptsChanged;

            _game.Init(
                GetNextWord()
            );

            CurrentAttempts = _settings.StartAttempts;
        }
    }
}