using UnityEngine;

namespace Interface
{
    public interface ICombatable
    {
        GameObject gameObject { get; }
        void SetTarget(Transform transform);
        public void TakeDamage(float damage);
        void TargetOnDead();
    }
}