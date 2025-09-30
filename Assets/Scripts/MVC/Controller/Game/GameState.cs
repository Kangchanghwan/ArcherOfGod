using UnityEngine;
using Interface;
using UnityEngine.Scripting;
using Util;

namespace MVC.Controller.Game
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
        private float _countdownTimer = 5f;

        public GameStartState(GameUIController gameUIController) : base(gameUIController)
        {
        }

        public override void Enter()
        {
            Debug.Log("Game Start State - Countdown begins!");
            _countdownTimer = 5f;
            GameUIController.ShowStartPanel(true);
            GameUIController.ShowEndPanel(false);

            // 초기 카운트다운 텍스트 설정
            GameUIController.UpdateCountdownText(Mathf.Ceil(_countdownTimer).ToString());
        }

        public override void Execute()
        {
            _countdownTimer -= Time.deltaTime;

            // 카운트다운 텍스트 업데이트
            int displayNumber = Mathf.CeilToInt(_countdownTimer);
            if (displayNumber > 0)
            {
                GameUIController.UpdateCountdownText(displayNumber.ToString());
            }
            else
            {
                GameUIController.UpdateCountdownText("START!");
            }

            // 카운트다운 완료 시 게임 플레이 상태로 전환
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
        private float _gameTimer = 90f;
        private const float GAME_TIME_LIMIT = 90f;


        public GamePlayingState(GameUIController gameUIController) : base(gameUIController)
        {
        }

        public override void Enter()
        {
            Debug.Log("Game Playing State - Game starts!");
            _gameTimer = GAME_TIME_LIMIT;
            GameUIController.ShowStartPanel(false);
            GameUIController.ShowEndPanel(false);
        }

        public override void Execute()
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

            GameUIController.UpdatePlayingTimerText(timeText);

            // 시간 초과 시 게임 종료
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

            // 시간 초과로 게임 종료 (패배 처리)
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

        public override void Execute()
        {
            // 게임 종료 상태에서는 특별한 업데이트 로직 없음
            // 필요시 재시작 버튼 처리 등은 GameController에서 처리
        }

        public override void Exit()
        {
            GameUIController.ShowEndPanel(false);
            Debug.Log("Restarting game...");

            // 게임 재시작을 위한 초기화 로직
            // EventManager.Clear(); // 필요시 이벤트 정리
        }
    }
}