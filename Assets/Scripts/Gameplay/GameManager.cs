﻿using UbiJam.Events;
using UbiJam.Inputs;
using UbiJam.Utils;
using UnityEngine.InputSystem;

namespace UbiJam.Gameplay
{
    /// <summary>
    /// Singleton used to keep track of the current game. Present in main scene only.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        /// <summary>
        /// Is the game currently paused ?
        /// </summary>
        public bool IsPaused { get; private set; } = false;

        /// <summary>
        /// Did the game start ?
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        /// <summary>
        /// Is the current game running ? Pass at true after timer displayed on screen and when the game isn't paused
        /// </summary>
        public bool IsRunning { get { return IsStarted && !IsPaused; } }

        public int CurrentScore { get; private set; } = 0;

        private GameTimer _gameTimer;

        protected override void Awake()
        {
            base.Awake();
            OnGameStarted.Listeners += StartGame;
            OnGameEnded.Listeners += StopGame;
        }

        private void Start()
        {
            _gameTimer = GameTimer.Instance;
            InputManager.Instance.SlingshotInputs.OnPauseAction.started += OnPauseInput;
            InputManager.Instance.CharacterInputs.OnPauseAction.started += OnPauseInput;
        }

        private void Update()
        {
            if (_gameTimer.RemainingGameTimeSeconds <= 0.0f)
            {
                new OnGameEnded();
                this.enabled = false;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnGameStarted.Listeners -= StartGame;
            OnGameEnded.Listeners -= StopGame;
        }

        public void AddPoint(int newPoint)
        {
            CurrentScore += newPoint;
        }

        private void StartGame(OnGameStarted _)
        {
            IsStarted = true;
        }

        private void StopGame(OnGameEnded _)
        {
            IsStarted = false;
        }

        /// <summary>
        /// Callback for when the user is clicking the pause button
        /// </summary>
        /// <param name="context"></param>
        private void OnPauseInput(InputAction.CallbackContext context)
        {
            IsPaused = !IsPaused;
        }
    }
}
