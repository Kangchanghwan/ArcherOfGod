using Component.Skill;
using Controller.Entity;
using Interface;
using UnityEngine;
using Util;

namespace MVC.Controller
{
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
        public EntityType EntityType { get; }
        public ICombatable Combatable { get; }
    
        public OnEntitySpawnEvent(EntityType entityType, ICombatable combatable)
        {
            EntityType = entityType;
            Combatable = combatable;
        }
    }
    
    public struct OnPlayingStartEvent : IEvent
    {
    }
    
}