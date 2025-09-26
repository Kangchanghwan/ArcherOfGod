using Controller.Entity;
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
    
    
}