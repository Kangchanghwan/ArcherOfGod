using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Component.Impact
{
    [System.Serializable]
    public struct WhirlWindCommand
    {
        public Transform target;
        public float pullRadius;
        public float swirlSpeed;
        public float pullStrength;
        public float randomness;
        public float whirlDuration;
        public float arrowsDuration;
    }

    public class WhirlWind: ImpactBase
    {
        private WhirlWindCommand _command;
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

        public void Initialize(WhirlWindCommand command)
        {
            _command = command;
        }
    
        protected override void OnImpact()
        {
            _timer += Time.deltaTime;
            if (_timer < .5f) return; // 회오리 치는 시간
            if (_timer > _command.whirlDuration) _released = true;
        
            if (_released == false)
                OnEffect();
            else
                OnRelease();
        }

        private void OnEffect()
        {
            foreach (var hits in Physics2D.OverlapCircleAll(transform.position, _command.pullRadius))
            {
                Arrow arrow = hits.GetComponent<Arrow>();
                
                if(arrow == null || _arrows.Contains(arrow)) continue;
                
                arrow.StopMoving();
                _arrows.Add(arrow);
            }
        
            foreach (var arrow in _arrows)
            {
                Vector2 dir = transform.position - arrow.transform.position;
                Vector2 tangent = new Vector2(-dir.y, dir.x).normalized;
            
                arrow.transform.position += (Vector3)tangent * (_command.swirlSpeed * Time.deltaTime);
                arrow.transform.position += (Vector3)dir.normalized * (_command.pullStrength * Time.deltaTime);
                arrow.transform.position += (Vector3)Random.insideUnitCircle * (_command.randomness * Time.deltaTime);
            }
        }
    
        private void OnRelease()
        {
            foreach (var arrow in _arrows)
                ShotArrow(arrow);
            ObjectPool.Instance.ReturnObject(gameObject).Forget();
        }

        private void ShotArrow(Arrow arrow)
        {
            Vector2 p0 = arrow.transform.position;
            Vector2 p1 = Vector2.up * 8f;
            Vector2 target = _command.target.position;
            Vector2 p2 = new Vector2(target.x + Random.Range(0f , _command.randomness), target.y); 
            arrow.duration = _command.arrowsDuration;
            UniTask.FromResult(arrow.Shoot(p0, p1, p2));
        }
        //
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, _command.pullRadius);    
        // }
    }
}