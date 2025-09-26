using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Interface;
using Manager;

namespace Component
{
    public class Arrow : MonoBehaviour
    {
        public float damage;
        public float duration;

        [SerializeField] private ParticleSystem particlePrefab;
        [SerializeField] private bool groundVfx;
        [SerializeField] private bool hasHitGround;

        private CancellationTokenSource _cancellationTokenSource;
        private Collider2D _collider2D;
        private ParticleSystem _particle;
        private float _elapsedTime;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            ResetArrow();
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
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && groundVfx)
            {
                ArrowParticle();
                if (hasHitGround)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void CheckPlayerOrEnemy(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
                other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var combatable = other.GetComponent<ICombatable>();
                OnDamage(combatable);
                ArrowParticle();
            }
        }

        private void ArrowParticle()
        {
            if (particlePrefab == null) return;
            if (_particle == null)
            {
                _particle = Instantiate(particlePrefab, transform.position, Quaternion.identity,
                    GameManager.Instance.transform);
                _particle.Play();
                return;
            }

            _particle.transform.position = transform.position;
            _particle.gameObject.SetActive(true);
            _particle.Play();
        }

        private void OnDamage(ICombatable damageable)
        {
            if (damageable == null) return;
            damageable.TakeDamage(damage);
            gameObject.SetActive(false);
            StopArrowTask();
        }

        public void StopArrowTask()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            _collider2D.enabled = false;
        }

        private void ResetArrow()
        {
            _elapsedTime = 0f;
            _collider2D.enabled = true;
        }

        private async UniTask ShotArrowTask(Vector2 p0, Vector2 p1, Vector2 p2, CancellationToken cancellationToken)
        {
            Vector2 previousPos = p0;
            p2.y = -1f;
            _elapsedTime = 0f;
            _collider2D.enabled = true;

            try
            {
                while (_elapsedTime < duration)
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

                _collider2D.enabled = false;
                await UniTask.Delay(4000, cancellationToken: cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    gameObject.SetActive(false);
                }
            }
            catch (OperationCanceledException)
            {
                // Task was cancelled, cleanup if needed
            }
        }

        public async void ShotArrow(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            ResetArrow();
            StopArrowTask(); // Stop any existing task

            _cancellationTokenSource = new CancellationTokenSource();
            await ShotArrowTask(p0, p1, p2, _cancellationTokenSource.Token);
        }
    }
}