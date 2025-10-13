using Interface;
using MVC.Controller;
using MVC.Controller.Enemy;
using MVC.Controller.Game;
using MVC.Controller.Player;
using UnityEngine;
using Util;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        // Controller
        [SerializeField] private GameUIController gameUIController;

        [SerializeField] private EnemyController enemyController;
        [SerializeField] private PlayerController playerController;

        //System
        private CombatSystem _combatSystem;

        protected override void Awake()
        {
            base.Awake();
            if (gameUIController == null)
                gameUIController = FindAnyObjectByType<GameUIController>();

            if (enemyController == null)
                enemyController = FindAnyObjectByType<EnemyController>();
            
            if (playerController == null)
                playerController = FindAnyObjectByType<PlayerController>();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<OnEntityDeathEvent>(HandleEntityDeath);
            EventManager.Subscribe<OnPlayingStartEvent>(HandleOnGameStart);
            EventManager.Subscribe<OnEntitySpawnEvent>(HandleOnEntitySpawn);
            EventManager.Subscribe<OnPlayingEndEvent>(HandleOnPlayEnd);
        }

        private void HandleOnPlayEnd(OnPlayingEndEvent obj)
        {
            playerController.ChangeIdleState();
            enemyController.ChangeIdleState();
        }
        private void HandleOnEntitySpawn(OnEntitySpawnEvent obj)
        {
            if (obj.EntityType == EntityType.CopyCat)
                _combatSystem.HandleCopyCatSpawn(obj.Combatable);
        }

        private void HandleOnGameStart(OnPlayingStartEvent obj)
        {
            playerController.OnCombatStart();
            enemyController.ChangeCastingState();
        }

        private void HandleEntityDeath(OnEntityDeathEvent obj)
        {
            _combatSystem.HandleCombatDeadRefresh(obj.Type);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<OnEntityDeathEvent>(HandleEntityDeath);
            EventManager.Unsubscribe<OnPlayingStartEvent>(HandleOnGameStart);
            EventManager.Unsubscribe<OnEntitySpawnEvent>(HandleOnEntitySpawn);
            EventManager.Unsubscribe<OnPlayingEndEvent>(HandleOnPlayEnd);

        }

        private void Start()
        {
            Debug.Log("=== GameManager Start BEGIN ===");
    
            try
            {
                Debug.Log("Initializing UI...");
                gameUIController.Init();
        
                Debug.Log("Initializing Player...");
                playerController.Init();

                Debug.Log("Initializing Enemy...");
                enemyController.Init();
       
                Debug.Log("Creating CombatSystem...");
                _combatSystem = new CombatSystem(
                    activeCombatants: new ICombatable[] { enemyController, playerController },
                    gameUIController: gameUIController);
                
                Debug.Log("=== GameManager Start COMPLETE ===");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Init failed: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}