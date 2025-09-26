using Controller.Entity;
using Interface;
using UnityEngine;
using Util;

namespace MVC.Controller
{
    public struct OnHealthUpdateEvent : IEvent
    {
        public readonly EntityType Type;
        public readonly int MaxHealth;
        public readonly int CurrentHealth;

        public OnHealthUpdateEvent(EntityType type, int maxHealth, int currentHealth)
        {
            Type = type;
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }

    public struct OnEntityDeathEvent : IEvent
    {
        public readonly EntityType Type;

        public OnEntityDeathEvent(EntityType type)
        {
            Type = type;
        }
    }

    public struct OnEntitySpawnEvent : IEvent
    {
        public readonly EntityType EntityType;
        public readonly ICombatable Combatable;
    
        public OnEntitySpawnEvent(EntityType entityType, ICombatable combatable)
        {
            EntityType = entityType;
            Combatable = combatable;
        }
    }
    
    public struct OnGameStartEvent : IEvent
    {
       
    }
    
    public class OnCombatEndEvent : IEvent
    {
        public readonly CombatResult Result;

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