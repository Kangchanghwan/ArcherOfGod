using System.Collections;
using UnityEngine;

namespace Component
{
    public class Arrow : MonoBehaviour
    {
        public float damage;
        public float duration;
    
        [SerializeField] private ParticleSystem particlePrefab;
        [SerializeField] private bool groundVfx;
        [SerializeField] private bool hasHitGround;

        private Coroutine _arrowCoroutine;
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
                var damageable = other.GetComponent<IDamageable>();
                OnDamage(damageable);
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

        private void OnDamage(IDamageable damageable)
        {
            if (damageable == null) return;
            damageable.TakeDamage(damage);
            gameObject.SetActive(false);
            StopArrowCoroutine();
            _arrowCoroutine = null;
        }

        public void StopArrowCoroutine()
        {
            if (_arrowCoroutine == null) return;
            StopCoroutine(_arrowCoroutine);
            _arrowCoroutine = null;
            _collider2D.enabled = false;
        }

        private void ResetArrow()
        {
            _elapsedTime = 0f;
            _collider2D.enabled = true;
        }


        private IEnumerator ShotArrowCoroutine(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Vector2 previousPos = p0;

            p2.y = -1f;
            _elapsedTime = 0f;

            while (_elapsedTime < duration)
            {
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
                yield return new WaitForEndOfFrame();
            }


            _collider2D.enabled = false;

            yield return new WaitForSeconds(4f);

            gameObject.SetActive(false);
        }

        public void ShotArrow(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            ResetArrow();
            _arrowCoroutine = StartCoroutine(ShotArrowCoroutine(p0, p1, p2));
        }
    
    }
}