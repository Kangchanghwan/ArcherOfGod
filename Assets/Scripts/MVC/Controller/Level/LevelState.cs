using Interface;
using UnityEngine;

namespace MVC.Controller.Level
{
    public abstract class GameStateBase : IState
    {
        protected readonly GameUIController GameUIController;
        protected GameStateBase(GameUIController gameUIController)
        {
            GameUIController = gameUIController;
        }

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
    
    public class GameStartState : GameStateBase
    {
        private float _countdownTimer;
        private readonly float _startCountdownTime;

        public GameStartState(GameUIController gameUIController, float startCountdownTime) : base(gameUIController)
        {
            _startCountdownTime = startCountdownTime;
        }

        public override void Enter()
        {
            Debug.Log("Game Start State - Countdown begins!");
            _countdownTimer = _startCountdownTime;
            GameUIController.ShowStartPanel(true);
            GameUIController.ShowEndPanel(false);

            GameUIController.UpdateCountdownText(Mathf.Ceil(_countdownTimer).ToString());
        }

        public override void Execute()
        {
            _countdownTimer -= Time.deltaTime;

            int displayNumber = Mathf.CeilToInt(_countdownTimer);
            if (displayNumber > 0)
            {
                GameUIController.UpdateCountdownText(displayNumber.ToString());
            }
            else
            {
                GameUIController.UpdateCountdownText("START!");
            }

            if (_countdownTimer <= 0f)
            {
                GameUIController.ChangeToGamePlaying();
            }
        }

        public override void Exit()
        {
            GameUIController.ShowStartPanel(false);
            Debug.Log("Countdown finished - Starting game!");
        }
    }

    class GamePlayingState : GameStateBase
    {
        private float _gameTimer;
        private readonly float _gameTimeLimit;

        public GamePlayingState(GameUIController gameUIController, float gameTimeLimit) : base(gameUIController)
        {
            _gameTimeLimit = gameTimeLimit;
        }

        public override void Enter()
        {
            Debug.Log("Game Playing State - Game starts!");
            _gameTimer = _gameTimeLimit;
            GameUIController.ShowStartPanel(false);
            GameUIController.ShowEndPanel(false);
        }

        public override void Execute()
        {
            _gameTimer -= Time.deltaTime;

            int totalSeconds = Mathf.FloorToInt(_gameTimer);
            string timeText = totalSeconds >= 10 ? totalSeconds.ToString() : totalSeconds.ToString("00");

            if (totalSeconds % 10 == 0 && _gameTimer % 1f < Time.deltaTime)
            {
                Debug.Log($"Remaining time: {timeText}");
            }

            GameUIController.UpdatePlayingTimerText(timeText);

            if (_gameTimer <= 0f)
            {
                TimeUp();
            }
        }

        public override void Exit()
        {
            Debug.Log("Game Playing State ended!");
        }

        private void TimeUp()
        {
            Debug.Log("Time's up! Game over by timeout.");
            GameUIController.ChangeToGameEndWithResult("Time Up!\nDefeat!", false);
        }
    }

    public class GameEndState : GameStateBase
    {
        private string _resultText = "Game Over";
        private bool _isVictory = false;

        public GameEndState(GameUIController gameUIController) : base(gameUIController)
        {
        }

        public void SetResult(string resultText, bool isVictory)
        {
            _resultText = resultText;
            _isVictory = isVictory;
        }

        public override void Enter()
        {
            Debug.Log($"Game End State - Result: {_resultText}");

            GameUIController.ShowStartPanel(false);
            GameUIController.ShowEndPanel(true);
            GameUIController.UpdateEndText(_resultText);

            if (_isVictory)
            {
                Debug.Log("Victory conditions met!");
            }
            else
            {
                Debug.Log("Defeat conditions met!");
            }
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            GameUIController.ShowEndPanel(false);
            Debug.Log("Restarting game...");
        }
    }
}