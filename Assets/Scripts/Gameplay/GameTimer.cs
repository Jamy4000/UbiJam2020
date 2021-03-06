﻿using System.Collections;
using System.Diagnostics;
using UbiJam.Events;
using UbiJam.Inputs;
using UbiJam.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UbiJam.Gameplay
{
    /// <summary>
    /// Handle the timer at the start of the game, as well as the running timer during the game
    /// </summary>
    public class GameTimer : MonoSingleton<GameTimer>
    {
        /// <summary>
        /// Reference to the Game Manager
        /// </summary>
        private GameManager _gameManager;

        /// <summary>
        /// The stopwatch used to calculate the user's time since the beginning of the game
        /// </summary>
        private Stopwatch _gameStopWatch = new Stopwatch();

        /// <summary>
        /// Elapsed time in milliseconds since the beginning of the game
        /// </summary>
        public float GameTime { get { return _gameStopWatch.ElapsedMilliseconds; } }

        /// <summary>
        /// Elapsed time in seconds since the beginning of the game
        /// </summary>
        public float GameTimeSeconds { get { return _gameStopWatch.ElapsedMilliseconds / 1000.0f; } }

        public float RemainingGameTimeSeconds { get { return _gameSettings.GameTime - GameTimeSeconds; } }

        private GameSettings _gameSettings;

        protected override void Awake()
        {
            base.Awake();
            OnGameEnded.Listeners += ResetTimerOnGameEnd;
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _gameSettings = GameSettings.Instance;

            InputManager.Instance.SlingshotInputs.OnPauseAction.started += OnPauseInput;
            InputManager.Instance.CharacterInputs.OnPauseAction.started += OnPauseInput;

            if (_gameSettings.UseCountdown)
                StartCoroutine(StartCountdown());
            else
                new OnGameStarted();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnGameEnded.Listeners -= ResetTimerOnGameEnd;
        }

        private IEnumerator StartCountdown()
        {
            _gameStopWatch.Start();

            while (GameTime < 6000.0f)
            {
                yield return new WaitForEndOfFrame();
            }

            _gameStopWatch.Restart();
            new OnGameStarted();
        }

        /// <summary>
        /// Update the different values in the SO at the end of the game
        /// </summary>
        private void ResetTimerOnGameEnd(OnGameEnded _)
        {
            _gameStopWatch.Stop();
            this.enabled = false;
        }

        /// <summary>
        /// Callback for when the user is clicking the pause button
        /// </summary>
        /// <param name="context"></param>
        public void OnPauseInput(InputAction.CallbackContext context)
        {
            if (_gameStopWatch.IsRunning)
                _gameStopWatch.Stop();
            else
                _gameStopWatch.Start();
        }

        public string GetCountdownText
        {
            get
            {
                switch ((int)GameTimeSeconds)
                {
                    case 0:
                    case 1:
                    case 2:
                        return _gameManager.IsStarted ? "GO GO GO GO" : "Prepare yourself ...";
                    case 3:
                        return "3";
                    case 4:
                        return "2";
                    case 5:
                        return "1";
                    default:
                        return "GO GO GO GO";
                }
            }
        }
    }
}
