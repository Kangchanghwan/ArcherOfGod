using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : ImpactBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player") || layer == LayerMask.NameToLayer("Bot"))
        {
            var damageAble = other.GetComponent<IDamageable>();
            if (damageAble == null) return;
            damageAble.TakeDamage(damage);
        }
    }

    protected override void OnImpact()
    {
        
    }
}
