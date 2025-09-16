using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    BombShoot,
    JumpShoot,
    RepidFire
}

public abstract class SkillBase : MonoBehaviour
{
    [Header("General detail")]
    [SerializeField] private float cooldown;
    [SerializeField] protected UISkillSlot uiSkillSlot;
    
    private float _lastTimeUsed;

    protected Rigidbody2D Rb;
    protected Transform Target;
    protected Animator Anim;

    protected Coroutine CurrentSkillCoroutine;

    protected Queue<GameObject> pools = new Queue<GameObject>();
    

 
    private void Start()
    {
        uiSkillSlot?.StartCooldown(cooldown);
    }
    
    public void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
    {
        this.Rb = rigidbody;
        this.Anim = anim;
        this.Target = target;
    }


    public bool CanUseSkill() => !OnCooldown();

    // 스킬 취소
    public void CancelSkill()
    {
        if (CurrentSkillCoroutine != null)
        {
            StopCoroutine(CurrentSkillCoroutine);

            Rb.bodyType = RigidbodyType2D.Dynamic;
            Rb.linearVelocity = Vector2.zero;
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


    private bool OnCooldown() => Time.time < _lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        _lastTimeUsed = Time.time;
        uiSkillSlot?.StartCooldown(cooldown);
    }

    public abstract IEnumerator SkillCoroutine();
    public abstract string GetAnimationName();
}