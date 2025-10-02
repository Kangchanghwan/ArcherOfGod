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
            _gameStartState = new GameStartState(this);
            _gamePlayingState = new GamePlayingState(this);
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
    
            // 구체 타입으로 캐스팅 시도
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
            _gameStartState = new GameStartState(this);
            _gamePlayingState = new GamePlayingState(this);
            _gameEndState = new GameEndState(this);
        }

        private void StartNewGame()
        {
            _stateMachine.ChangeState(_gameStartState);
        }

        // State 전환 메서드들
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
            _gameEndState.SetResult(resultText, isVictory);
            _stateMachine.ChangeState(_gameEndState);
        }

        // UI 업데이트 메서드들
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