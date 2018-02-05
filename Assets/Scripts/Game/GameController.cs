using System;
using Game.Interfaces;
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

        private IWordsProvider _wordProvider;

        private int CurrentAttempts
        {
            get { return _currentAttempts; }
            set
            {
                _currentAttempts = value;
                if (AttemptsCountChanged != null) AttemptsCountChanged.Invoke(value);
                if (value >= 0) return;
                if (LooseEvent != null) LooseEvent.Invoke();
            }
        }

        private event Action WinEvent;
        private event Action LooseEvent;
        private event Action<int> AttemptsCountChanged;


        private void OnWin()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            _wordProvider.Reset();
            CurrentAttempts = _settings.StartAttempts;
            NewLevel();
        }

        private void OnLevelComplete()
        {
            CurrentAttempts += CurrentAttempts * 2;
            NewLevel();
        }

        private void NewLevel()
        {
            var nextWord = NextWord();
            if (nextWord == null)
            {
                if (WinEvent != null) WinEvent.Invoke();
            }
            else
            {
                _game.Init(nextWord);
            }

            _input.OnNewLevel();
        }

        private void OnLoose()
        {
            ResetGame();
        }

        private void OnLetterMiss()
        {
            --CurrentAttempts;
        }


        private string NextWord()
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

            LooseEvent += OnLoose;
            LooseEvent += _gameInterface.OnLoose;

            AttemptsCountChanged += _gameInterface.OnAttemptsChanged;

            _game.Init(
                NextWord()
            );

            CurrentAttempts = _settings.StartAttempts;
        }
    }
}