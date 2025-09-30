using UnityEngine;
using Interface;
using Util;

namespace MVC.Controller.Game
{
    public class GameStartState : IState
    {
        private readonly GameUIController _gameUIController;
        private float _countdownTimer = 5f;

        public GameStartState(GameUIController gameUIController)
        {
            _gameUIController = gameUIController;
        }

        public void Enter()
        {
            Debug.Log("Game Start State - Countdown begins!");
            _countdownTimer = 5f;
            _gameUIController.ShowStartPanel(true);
            _gameUIController.ShowEndPanel(false);

            // 초기 카운트다운 텍스트 설정
            _gameUIController.UpdateCountdownText(Mathf.Ceil(_countdownTimer).ToString());
        }

        public void Execute()
        {
            _countdownTimer -= Time.deltaTime;

            // 카운트다운 텍스트 업데이트
            int displayNumber = Mathf.CeilToInt(_countdownTimer);
            if (displayNumber > 0)
            {
                _gameUIController.UpdateCountdownText(displayNumber.ToString());
            }
            else
            {
                _gameUIController.UpdateCountdownText("START!");
            }

            // 카운트다운 완료 시 게임 플레이 상태로 전환
            if (_countdownTimer <= 0f)
            {
                _gameUIController.ChangeToGamePlaying();
            }
        }

        public void Exit()
        {
            _gameUIController.ShowStartPanel(false);
            Debug.Log("Countdown finished - Starting game!");
        }
    }

    class GamePlayingState : IState
    {
        private readonly GameUIController _gameUIController;
        private float _gameTimer = 90f;
        private const float GAME_TIME_LIMIT = 90f;

        public GamePlayingState(GameUIController gameUIController)
        {
            _gameUIController = gameUIController;
        }

        public void Enter()
        {
            Debug.Log("Game Playing State - Game starts!");
            _gameTimer = GAME_TIME_LIMIT;
            _gameUIController.ShowStartPanel(false);
            _gameUIController.ShowEndPanel(false);
        }

        public void Execute()
        {
            _gameTimer -= Time.deltaTime;

            // 전체 남은 시간을 초 단위로 표시
            int totalSeconds = Mathf.FloorToInt(_gameTimer);

            // 10초 미만일 때는 "09", "08" 형태로, 10초 이상일 때는 "90", "89" 형태로 표시
            string timeText = totalSeconds >= 10 ? totalSeconds.ToString() : totalSeconds.ToString("00");

            // Debug로 시간 확인 (실제로는 UI 텍스트에 표시)
            if (totalSeconds % 10 == 0 && _gameTimer % 1f < Time.deltaTime)
            {
                Debug.Log($"Remaining time: {timeText}");
            }

            _gameUIController.UpdatePlayingTimerText(timeText);

            // 시간 초과 시 게임 종료
            if (_gameTimer <= 0f)
            {
                TimeUp();
            }
        }

        public void Exit()
        {
            Debug.Log("Game Playing State ended!");
        }

        private void TimeUp()
        {
            Debug.Log("Time's up! Game over by timeout.");

            // 시간 초과로 게임 종료 (패배 처리)
            _gameUIController.ChangeToGameEndWithResult("Time Up!\nDefeat!", false);
        }
    }

    public class GameEndState : IState
    {
        private readonly GameUIController _gameUIController;
        private string _resultText = "Game Over";
        private bool _isVictory = false;

        public GameEndState(GameUIController gameUIController)
        {
            _gameUIController = gameUIController;
        }

        public void SetResult(string resultText, bool isVictory)
        {
            _resultText = resultText;
            _isVictory = isVictory;
        }

        public void Enter()
        {
            Debug.Log($"Game End State - Result: {_resultText}");

            _gameUIController.ShowStartPanel(false);
            _gameUIController.ShowEndPanel(true);
            _gameUIController.UpdateEndText(_resultText);

            // 게임 종료 시 플레이어 입력 비활성화 등 처리 가능
            // 승리/패배에 따른 추가 로직 처리
            if (_isVictory)
            {
                Debug.Log("Victory conditions met!");
                // 승리 시 추가 로직 (점수 저장, 다음 레벨 등)
            }
            else
            {
                Debug.Log("Defeat conditions met!");
                // 패배 시 추가 로직 (재시작 권유 등)
            }
        }

        public void Execute()
        {
            // 게임 종료 상태에서는 특별한 업데이트 로직 없음
            // 필요시 재시작 버튼 처리 등은 GameController에서 처리
        }

        public void Exit()
        {
            _gameUIController.ShowEndPanel(false);
            Debug.Log("Restarting game...");

            // 게임 재시작을 위한 초기화 로직
            // EventManager.Clear(); // 필요시 이벤트 정리
        }
    }
}