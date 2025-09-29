using System.Collections.Generic;
using Controller.Entity;
using Interface;
using MVC.Controller;
using MVC.Controller.CopyCat;
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
        }

        private void Start()
        {
            gameUIController.Init();
            enemyController.Init();
            playerController.Init();

            _combatSystem = new CombatSystem(
                activeCombatants: new ICombatable[] { enemyController, playerController },
                gameUIController: gameUIController);
        }
    }
}