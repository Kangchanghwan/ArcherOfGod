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

        private void Awake()
        {
            EventManager.Subscribe<OnEntitySpawnEvent>(RegisterCombatant);
            EventManager.Subscribe<OnEntityDeathEvent>(HandleEntityDeath);
        }

        private void Start()
        {
            TargetSetting();
        }

        private void TargetSetting()
        {
            _activeCombatants[EntityType.Player]
                .SetTarget(_activeCombatants[EntityType.Enemy].gameObject.transform);
            _activeCombatants[EntityType.Enemy]
                .SetTarget(_activeCombatants[EntityType.Player].gameObject.transform);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<OnEntitySpawnEvent>(RegisterCombatant);
            EventManager.Unsubscribe<OnEntityDeathEvent>(HandleEntityDeath);
        }

        private void RegisterCombatant(OnEntitySpawnEvent @event)
        {
            _activeCombatants[@event.EntityType] = @event.Combatable;
            switch (@event.EntityType)
            {
                case EntityType.CopyCat:
                    _activeCombatants[EntityType.CopyCat]
                        .SetTarget(_activeCombatants[EntityType.Enemy].gameObject.transform);
                    _activeCombatants[EntityType.Enemy]
                        .SetTarget(_activeCombatants[EntityType.CopyCat].gameObject.transform);
                    break;
            }

            Debug.Log($"Combat participant registered: {@event.EntityType}");
        }

        private void HandleEntityDeath(OnEntityDeathEvent @event)
        {
            if (_activeCombatants.ContainsKey(@event.Type))
            {
                _activeCombatants.Remove(@event.Type);
                Debug.Log($"Combat participant removed: {@event.Type}");

                // 다른 전투 참가자들에게 타겟 사망 알림
                if (@event.Type == EntityType.CopyCat)
                {
                    _activeCombatants[EntityType.Enemy]
                        .SetTarget(_activeCombatants[EntityType.Player].gameObject.transform);
                    return;
                }
                NotifyTargetDeath();

                // 전투 종료 체크
                CheckCombatEnd();
            }
        }

        private void NotifyTargetDeath()
        {
            foreach (var combatant in _activeCombatants.Values)
            {
                combatant.TargetOnDead();
            }
        }

        private void CheckCombatEnd()
        {
            if (!_activeCombatants.ContainsKey(EntityType.Player))
            {
                // 플레이어 패배
                EventManager.Publish(new OnCombatEndEvent(CombatResult.Defeat));
            }
            else if (_activeCombatants.Count == 1) // 플레이어만 남음
            {
                // 플레이어 승리
                EventManager.Publish(new OnCombatEndEvent(CombatResult.Victory));
            }
        }

        /// <summary>
        /// 현재 활성 전투 참가자 수 반환
        /// </summary>
        public int GetActiveCombatantCount() => _activeCombatants.Count;

        /// <summary>
        /// 특정 타입의 전투 참가자가 활성 상태인지 확인
        /// </summary>
        public bool IsEntityActive(EntityType entityType) => _activeCombatants.ContainsKey(entityType);
    }

    public class OnCombatEndEvent : IEvent
    {
        public CombatResult Result { get; }

        public OnCombatEndEvent(CombatResult result)
        {
            Result = result;
        }
    }

    public enum CombatResult
    {
        Victory,
        Defeat
    }
}