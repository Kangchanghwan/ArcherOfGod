using MVC.Controller;
using UnityEngine;

namespace Interface
{
    public interface ICombatable
    {
        EntityType GetEntityType();
        void SetTarget(Transform transform);
        public void TakeDamage(float damage);
        void TargetOnDead();
    }
}