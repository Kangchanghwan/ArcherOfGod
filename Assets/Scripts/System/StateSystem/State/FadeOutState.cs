using System.Collections;
using UnityEngine;

public class FadeOutState: StateBase
{
    [SerializeField]
    private float fadeTime = 0.5f;

    public bool IsDone { get; private set; }

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    public override void Enter()
    {
        IsDone = false;
        base.Enter();
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

        IsDone = true;
    }
    
    public override void Exit()
    {
        base.Exit();
    }

    protected override string GetAnimationName() => "Idle";
}
