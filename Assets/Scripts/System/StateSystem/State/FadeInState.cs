using System.Collections;
using UnityEngine;

public class FadeInState : StateBase
{
    public float fadeTime = 0.5f;
 
    public bool isDone { get; private set; }
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    protected override string GetAnimationName() => "Idle";
    

    public override void Enter()
    {
        base.Enter();
        var color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        var time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            var spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = Mathf.Lerp(0f, 1f, time / fadeTime);
            spriteRenderer.color = spriteRendererColor;
            yield return null;
        }
        
        isDone = true;
    }
    
    public override void Exit()
    {
        isDone = false;
    }
}
