using UnityEngine;
using UnityEngine.UI;
using Util;
using TMPro;

namespace MVC.Controller.Game
{
    public class GameUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI countDownTimer;
        [SerializeField] private GameObject gameStartPanel;
        [SerializeField] private TextMeshProUGUI playingTimer;
        [SerializeField] private GameObject gameEndPanel;
        [SerializeField] private TextMeshProUGUI gameEndText;
        [SerializeField] private Button loftButton;

        [Header("Game Settings")]
        [SerializeField] private float startCountdownTime = 5f;
        [SerializeField] private float gameTimeLimit = 90f;

        private StateMachine _stateMachine;
        private GameStartState _gameStartState;
        private GamePlayingState _gamePlayingState;
        private GameEndState _gameEndState;

        public void Init()
        {
            InitializeStateMachine();
            InitializeStates();
            StartStateMachine();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
        }

        private void InitializeStates()
        {
            _gameStartState = new GameStartState(this, startCountdownTime);
            _gamePlayingState = new GamePlayingState(this, gameTimeLimit);
            _gameEndState = new GameEndState(this);
        }

        private void StartStateMachine()
        {
            _stateMachine.Initialize(_gameStartState);
        }

        private void Update()
        {
            if (_stateMachine?.CurrentState == null)
                return;

            if (_stateMachine.CurrentState is EntityStateBase state)
                state.Execute();
            else
                _stateMachine.CurrentState.Execute();
        }

        private void OnEnable()
        {
            if (loftButton != null)
                loftButton.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            if (loftButton != null)
                loftButton.onClick.RemoveListener(RestartGame);
        }

        public void Win()
        {
            Debug.Log("Victory!");
            _gameEndState.SetResult("Victory!", true);
            _stateMachine.ChangeState(_gameEndState);
        }
        
        public void Lose()
        {
            Debug.Log("Game Over - You Lose!");
            _gameEndState.SetResult("Defeat!", false);
            _stateMachine.ChangeState(_gameEndState);
        }
        
        private void RestartGame()
        {
            ResetGameStates();
            StartNewGame();
        }

        private void ResetGameStates()
        {
            _gameStartState = new GameStartState(this, startCountdownTime);
            _gamePlayingState = new GamePlayingState(this, gameTimeLimit);
            _gameEndState = new GameEndState(this);
        }

        private void StartNewGame()
        {
            _stateMachine.ChangeState(_gameStartState);
        }

        public void ChangeToGamePlaying()
        {
            EventManager.Publish<OnPlayingStartEvent>();
            _stateMachine.ChangeState(_gamePlayingState);
        }

        public void ChangeToGameEnd()
        {
            _stateMachine.ChangeState(_gameEndState);
        }

        public void ChangeToGameEndWithResult(string resultText, bool isVictory)
        {
            EventManager.Publish<OnPlayingEndEvent>();
            _gameEndState.SetResult(resultText, isVictory);
            ChangeToGameEnd();
        }

        public void UpdateCountdownText(string text) 
        {
            if (countDownTimer != null)
                countDownTimer.text = text;
        }

        public void UpdatePlayingTimerText(string text) 
        {
            if (playingTimer != null)
                playingTimer.text = text;
        }
        
        public void ShowStartPanel(bool show) 
        {
            if (gameStartPanel != null)
                gameStartPanel.SetActive(show);
        }
        
        public void ShowEndPanel(bool show) 
        {
            if (gameEndPanel != null)
                gameEndPanel.SetActive(show);
        }
        
        public void UpdateEndText(string text) 
        {
            if (gameEndText != null)
                gameEndText.text = text;
        }
    }
}