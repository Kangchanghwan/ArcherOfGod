using UnityEngine;

public abstract class ImpactBase : MonoBehaviour
{
    [Header("ImpactArrowSettings")]
    [SerializeField] protected float damage = 10f;

    protected void Update()
    {
        OnImpact();
    }

    protected abstract void OnImpact();
}
