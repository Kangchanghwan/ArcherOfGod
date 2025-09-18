using System.Collections;
using UnityEngine;

public class FadeOutState: StateBase
{
    [SerializeField]
    private float fadeTime = 0.5f;
    private IDamageable _damageable;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    public override void Enter()
    {
        base.Enter();
        if (Entity is IDamageable entity)
            _damageable = entity;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            var spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = Mathf.Lerp(1f, 0f, time / fadeTime);
            spriteRenderer.color = spriteRendererColor;
            yield return null;
        }
        Entity.gameObject.SetActive(false);
    }
    
    public override void Exit()
    {
        base.Exit();
    }

    protected override string GetAnimationName() => "Idle";
}
