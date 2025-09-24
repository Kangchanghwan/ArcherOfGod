using UnityEngine;

namespace Controller.Entity
{
    public abstract class EntityBase : MonoBehaviour
    {
        [SerializeField]
        public Transform Target;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
    
        private bool _facingTarget;
    
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
        
            Debug.Assert(Animator != null);
            Debug.Assert(Rigidbody2D != null);
        }

        public void FlipController(float x = 0)
        {
            if (x >= 0 && !_facingTarget)
            {
                Flip();
            }
            else if (x <= 0 && _facingTarget)
            {
                Flip();
            }
        }
        public void FaceTarget()
        {
            if (Target != null)
            {
                Vector3 directionToEnemy = Target.position - transform.position;
                if (directionToEnemy.x > 0 && !_facingTarget)
                    Flip();
                else if (directionToEnemy.x < 0 && _facingTarget)
                    Flip();
            }
        }
        private void Flip()
        {
            _facingTarget = !_facingTarget;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}