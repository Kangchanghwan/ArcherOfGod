using System.Collections.Generic;
using Controller.Entity;
using Interface;
using MVC.Controller;
using UnityEngine;
using Util;

namespace Manager
{
   public class CombatManager : MonoBehaviour
    {
        private readonly Dictionary<EntityType, ICombatable> _activeCombatants = new();
        private readonly HashSet<EntityType> _expectedEntities = new() { EntityType.Player, EntityType.Enemy };
        
        private void Awake()
        {
            EventManager.Subscribe<OnEntitySpawnEvent>(RegisterCombatant);
            EventManager.Subscribe<OnEntityDeathEvent>(HandleEntityDeath);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<OnEntitySpawnEvent>(RegisterCombatant);
            EventManager.Unsubscribe<OnEntityDeathEvent>(HandleEntityDeath);
        }

        private void RegisterCombatant(OnEntitySpawnEvent @event)
        {
            _activeCombatants[@event.EntityType] = @event.Combatable;
            Debug.Log($"Combat participant registered: {@event.EntityType}");
            
            // 모든 필수 엔티티가 등록되면 초기 타겟팅 설정
            if (AreAllExpectedEntitiesRegistered())
            {
                SetInitialTargeting();
            }
            
            // CopyCat 등록 시 동적 타겟팅
            if (@event.EntityType == EntityType.CopyCat)
            {
                HandleCopyCatSpawn();
            }
        }

        private bool AreAllExpectedEntitiesRegistered()
        {
            foreach (var entityType in _expectedEntities)
            {
                if (!_activeCombatants.ContainsKey(entityType))
                    return false;
            }
            return true;
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

        private void HandleCopyCatSpawn()
        {
            if (TryGetCombatant(EntityType.CopyCat, out var copyCat) && 
                TryGetCombatant(EntityType.Enemy, out var enemy))
            {
                // CopyCat은 Enemy를 타겟, Enemy는 CopyCat을 타겟
                copyCat.SetTarget(GetTransform(enemy));
                enemy.SetTarget(GetTransform(copyCat));
                Debug.Log("CopyCat targeting established");
            }
        }

        private void HandleEntityDeath(OnEntityDeathEvent @event)
        {
            if (!_activeCombatants.ContainsKey(@event.Type)) return;

            _activeCombatants.Remove(@event.Type);
            Debug.Log($"Combat participant removed: {@event.Type}");

            // 타겟 재설정
            RetargetAfterDeath(@event.Type);
            
            // 전투 종료 체크
            CheckCombatEnd(@event.Type);
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
                    EventManager.Publish(new OnCombatEndEvent(CombatResult.Defeat));
                    break;
                case EntityType.Enemy:
                    EventManager.Publish(new OnCombatEndEvent(CombatResult.Victory));
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