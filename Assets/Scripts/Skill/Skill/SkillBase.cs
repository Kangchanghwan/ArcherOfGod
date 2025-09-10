using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("General detail")] [SerializeField]
    private float cooldown;
    [SerializeField] private float lastTimeUsed = 0f;

    protected Rigidbody2D rb;
    protected Transform target;
    protected Animator anim;

    protected Coroutine currentSkillCoroutine;

    protected Queue<GameObject> pools = new Queue<GameObject>();

    protected void Awake()
    {
        lastTimeUsed = cooldown;
    }

    public void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
    {
        this.rb = rigidbody;
        this.anim = anim;
        this.target = target;
    }


    public bool CanUseSkill()
    {
        if (OnCooldown())
        {
            return false;
        }

        return true;
    }

    // 스킬 취소
    public void CancelSkill()
    {
        if (currentSkillCoroutine != null)
        {
            StopCoroutine(currentSkillCoroutine);

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected GameObject PoolObject(GameObject prefab)
    {
        GameObject pool = null;

        if (pools.Count > 0)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                pool = pools.Dequeue();
                if (pool.activeSelf == false)
                {
                    pools.Enqueue(pool);
                    pool.SetActive(true);
                    return pool;
                }

                pools.Enqueue(pool);
            }
        }

        pool = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, -180f), GameManager.Instance.transform);
        pools.Enqueue(pool);
        pool.SetActive(true);
        return pool;
    }


    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;

    public abstract IEnumerator SkillCoroutine();
    public abstract string GetAnimationName();
}