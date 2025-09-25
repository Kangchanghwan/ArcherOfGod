using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Component.Impact
{
    public class WhirlWind: ImpactBase
    {
        [SerializeField]
        private Transform _target;
    
        [Header("Whirlwind Settings")]
        [SerializeField] private float pullRadius = 3f;
        [SerializeField] private float swirlSpeed = 2f;
        [SerializeField] private float pullStrength = 0.5f;
        [SerializeField] private float randomness = 0.5f;
        [SerializeField] private float whirlDuration = 10f;


        private readonly List<Arrow> _arrows = new();
        private float _timer;
        [SerializeField]
        private bool _released;
    
        private void OnEnable()
        {   
            _timer = 0f;
            _released = false;
            _arrows.Clear();
        }
    
        public void SetTarget(Transform target) => _target = target;
    
        protected override void OnImpact()
        {
            _timer +=  Time.deltaTime;
            if (_timer < .5f) return; // 회오리 치는 시간
            if (_timer > whirlDuration) _released = true;
        
            if (_released == false)
                OnEffect();
            else
                OnRelease();
        }
        private void OnEffect()
        {

            foreach (var hitArrow in  Physics2D.OverlapCircleAll(transform.position, pullRadius))
            {
                Arrow arrow = hitArrow.GetComponent<Arrow>();
                if(arrow == null || _arrows.Contains(arrow)) continue;
            
                _arrows.Add(arrow);
                arrow.StopArrowCoroutine();
            }
        
            foreach (var arrow in _arrows)
            {
                Vector2 dir = transform.position - arrow.transform.position;
                Vector2 tangent = new Vector2(-dir.y, dir.x).normalized;
            
                arrow.transform.position += (Vector3)tangent * (swirlSpeed * Time.deltaTime);
                arrow.transform.position += (Vector3)dir.normalized * (pullStrength * Time.deltaTime);
                arrow.transform.position += (Vector3)Random.insideUnitCircle * (randomness * Time.deltaTime);
            }
        }
    
    
        private void OnRelease()
        {
            foreach (var arrow in _arrows)
                ShotArrow(arrow);
            gameObject.SetActive(false);
        }


        private void ShotArrow(Arrow arrow)
        {
            Vector2 p0 = arrow.transform.position;
            Vector2 p1 = Vector2.up * 8f;
            Vector2 target = _target.position;
            Vector2 p2 = target; 
            //new Vector2(Random.Range(-3f, 3f), 0f);

            arrow.ShotArrow(p0, p1, p2);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, pullRadius);    
        // }
    }
}
