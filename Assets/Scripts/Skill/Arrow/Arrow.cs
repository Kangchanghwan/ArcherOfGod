using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [field: Header("Arrow")]
    [field: SerializeField] public bool TargetPlayerOrBot { get; set; }
    [SerializeField] private float damage;
    [field: SerializeField] public float Duration { get; set; }
    [SerializeField] private ParticleSystem particlePrefab;
    private float elapsedTime = 0f;

    [SerializeField] private bool groundVfx;
    [SerializeField] private bool hasHitGround;

    private Coroutine arrowCoroutine;

    private Collider2D collider2D;
    
    private ParticleSystem particle;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ResetArrow();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable;
        switch (TargetPlayerOrBot)
        {
            case true:
                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    damageable = other.GetComponent<IDamageable>();
                    DamageAble(damageable);
                    ArrowParticle();
                }
                break;
            case false:
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    damageable = other.GetComponent<IDamageable>();
                    DamageAble(damageable);
                    ArrowParticle();
                }
                break;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && groundVfx)
        {
            ArrowParticle();
            if (hasHitGround)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void ArrowParticle()
    {
        if (particlePrefab == null) return;
        if (particle == null)
        {
            particle = Instantiate(particlePrefab, transform.position, Quaternion.identity, GameManager.Instance.transform);
            particle.Play();
            return;
        }
        
        particle.transform.position = transform.position;
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    private void DamageAble(IDamageable damageable)
    {
        if (damageable == null) return;
        damageable.TakeDamage(damage);
        gameObject.SetActive(false);
        StopArrowCoroutine();
        arrowCoroutine = null;
    }
    
    public void StopArrowCoroutine()
    {
        if (arrowCoroutine == null) return;
        StopCoroutine(arrowCoroutine);
        arrowCoroutine = null;
        collider2D.enabled = false;
    }

    public void ResetArrow()
    {
        elapsedTime = 0f;
        collider2D.enabled = true;
    }
    
    public void ShotArrow(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        ResetArrow();
        arrowCoroutine = StartCoroutine(ShotArrowCoroutine(p0, p1, p2));
    }

    private IEnumerator ShotArrowCoroutine(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        Vector2 previousPos = p0;
        
        p2.y = 0f;
        
        elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / Duration;

            Vector2 pos = BezierCurve.Quadratic(p0, p1, p2, t);
            transform.position = pos;
        
            Vector2 direction = pos - previousPos;
            if (direction != Vector2.zero)
            {
                // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.up = direction.normalized;
            }

            previousPos = pos;
            yield return new WaitForEndOfFrame();
        }

        if (p2.x >= -1.9f && p2.x <= 1.9f)
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = new Vector2(p2.x, -8f);
            float dropDuration = 0.5f;
            float dropTime = 0f;

            while (dropTime < dropDuration)
            {
                dropTime += Time.deltaTime;
                float t = dropTime / dropDuration;
                transform.position = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
        
        collider2D.enabled = false;
    
        yield return new WaitForSeconds(4f);
    
        gameObject.SetActive(false);
    }

    
}