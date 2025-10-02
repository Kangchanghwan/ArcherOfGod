using System.Collections.Generic;
using System.Linq;
using Interface;
using MVC.Controller;
using MVC.Controller.Game;
using UnityEngine;
using UnityEngine.Scripting;
using Util;

namespace Manager
{
    [Preserve]
   public class CombatSystem
   {
       private readonly Dictionary<EntityType, ICombatable> _activeCombatants;
       private readonly GameUIController _gameUIController;

       public CombatSystem(ICombatable[] activeCombatants, GameUIController gameUIController)
       {
           _activeCombatants = activeCombatants.ToDictionary(
               c => c.GetEntityType(),
               c => c
           );
           _gameUIController = gameUIController;

           SetInitialTargeting();
       }

       private void SetInitialTargeting()
        {
            if (TryGetCombatant(EntityType.Player, out var player) && 
                TryGetCombatant(EntityType.Enemy, out var enemy))
            {
                player.SetTarget(GetTransform(enemy));
                enemy.SetTarget(GetTransform(player));
                Debug.Log("Initial targeting set between Player and Enemy");
            }
        }

        public void HandleCopyCatSpawn(ICombatable copyCat)
        {

            if (TryGetCombatant(EntityType.Enemy, out var enemy))
            {
                // CopyCat은 Enemy를 타겟, Enemy는 CopyCat을 타겟
                copyCat.SetTarget(GetTransform(enemy));
                enemy.SetTarget(GetTransform(copyCat));
                Debug.Log("CopyCat targeting established");
            }
        }

        public void HandleCombatDeadRefresh(EntityType entityType)
        {
            _activeCombatants.Remove(entityType);

            // 타겟 재설정
            RetargetAfterDeath(entityType);
            
            // 전투 종료 체크
            CheckCombatEnd(entityType);
        }

        private void RetargetAfterDeath(EntityType deadType)
        {
            switch (deadType)
            {
                case EntityType.CopyCat when TryGetCombatant(EntityType.Enemy, out var enemy) 
                                           && TryGetCombatant(EntityType.Player, out var player):
                    // CopyCat 죽으면 Enemy가 다시 Player를 타겟
                    enemy.SetTarget(GetTransform(player));
                    break;
                    
                case EntityType.Enemy : case EntityType.Player:
                    // Player or Enemy 죽으면 모든 생존자들에게 타겟 사망 알림
                    NotifyTargetDeath();
                    break;
            }
        }

        private void NotifyTargetDeath()
        {
            foreach (var combatant in _activeCombatants.Values)
            {
                combatant.TargetOnDead();
            }
        }

        private void CheckCombatEnd(EntityType deadType)
        {
            switch (deadType)
            {
                case EntityType.Player:
                    _gameUIController.Lose();
                    break;
                case EntityType.Enemy:
                    _gameUIController.Win();
                    break;
            }
        }

        private bool TryGetCombatant(EntityType type, out ICombatable combatant)
        {
            return _activeCombatants.TryGetValue(type, out combatant);
        }

        private Transform GetTransform(ICombatable combatant)
        {
            return ((MonoBehaviour)combatant).transform;
        }

        public int GetActiveCombatantCount() => _activeCombatants.Count;
        public bool IsEntityActive(EntityType entityType) => _activeCombatants.ContainsKey(entityType);
    }
   
}