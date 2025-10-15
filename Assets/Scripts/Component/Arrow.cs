using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Interface;
using Manager;

namespace Component
{
    public class Arrow : MonoBehaviour
    {
        public float damage;
        public float duration;

        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject groundHitEffect;

        private CancellationTokenSource _cancellationTokenSource;
        private Collider2D _collider2D;
  
        private float _elapsedTime;
        private bool _onMove;
        private bool _isGround;
        
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _collider2D.enabled = true;
            _isGround = false;
        }

        private void OnDisable()
        {
            StopArrowTask();
        }

        private void OnDestroy()
        {
            StopArrowTask();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckPlayerOrEnemy(other);
            CheckGround(other);
        }

        private void CheckGround(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (groundHitEffect != null)
                {
                    PlayParticle(groundHitEffect);
                }
                _collider2D.enabled = false;
                _isGround = true;
            }
        }

        private void CheckPlayerOrEnemy(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
                other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var combatable = other.GetComponent<ICombatable>();
                OnDamage(combatable);
                StopArrowTask();
                PlayParticle(hitEffect);
            }
        }

        private void PlayParticle(GameObject prefab)
        {
            if (hitEffect == null) return;
            var particle = ObjectPool.Instance.GetObject(prefab, transform).GetComponent<ParticleSystem>();
            particle.Play();
        }

        private void OnDamage(ICombatable damageable)
        {
            if (damageable == null) return;
            damageable.TakeDamage(damage);
        }

        private void StopArrowTask()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void StopMoving()
        {
            _onMove = false;
            _collider2D.enabled = false;
        }

        private async UniTask ShotArrowTask(Vector2 p0, Vector2 p1, Vector2 p2, CancellationToken cancellationToken)
        {
            Vector2 previousPos = p0;
            p2.y = -1f;
            _elapsedTime = 0f;
            _onMove = true;
            _collider2D.enabled = true;

            try
            {
                while (_elapsedTime < duration && _onMove)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    _elapsedTime += Time.deltaTime;
                    float t = _elapsedTime / duration;

                    Vector2 pos = BezierCurve.Quadratic(p0, p1, p2, t);
                    transform.position = pos;

                    Vector2 direction = pos - previousPos;
                    if (direction != Vector2.zero)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0f, 0f, angle);
                    }
                    
                    previousPos = pos;
                    await UniTask.NextFrame(cancellationToken);
                }

                if (_isGround)
                {
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                    await ObjectPool.Instance.ReturnObject(gameObject);
                    _cancellationTokenSource?.Cancel();  
                }

            }
            catch (OperationCanceledException)
            {
                await ObjectPool.Instance.ReturnObject(gameObject);
            }
        }

        public async UniTask Shoot(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await ShotArrowTask(p0, p1, p2, _cancellationTokenSource.Token);
        }
    }
}