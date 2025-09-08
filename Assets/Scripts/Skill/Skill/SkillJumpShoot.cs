using UnityEngine;
using System.Collections;

public class SkillJumpShoot : SkillBase
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;

    private Vector3 _originalPosition;

    private bool _up;
    private bool _hover;
    private bool _down;
    private bool _left;
    
    public override IEnumerator SkillCoroutine()
    {
        _up = false;
        _hover = false;
        _down = false;
        _left = false;
        _originalPosition = rb.transform.position;
        
        print("SkillCoroutine() 실행");
        
        currentSkillCoroutine = StartCoroutine(ExecuteJump());
        yield return currentSkillCoroutine;
    }

    private IEnumerator ExecuteJump()
    {
        // 상승 전 대기 단계
        yield return new WaitUntil(() => _up); // up이 참이 될때까지 기다림
        // 상승 단계
        yield return StartCoroutine(RisePhase());
        // 공중에서 정지 단계
        yield return new  WaitUntil(() => _down);
        // 하강 단계
        yield return StartCoroutine(FallPhase());
    }

  
    private IEnumerator RisePhase()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        float peak = _originalPosition.y + jumpHeight;
        rb.transform.position = new Vector2(_originalPosition.x, peak);
        yield return new WaitUntil(() => _hover); 
    }


    private IEnumerator FallPhase()
    {
        rb.transform.position = new Vector2(_originalPosition.x, _originalPosition.y);
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitUntil(() => _left);
    }
    
    public void OnJumpReadyTrigger() => Debug.Log("OnJumpReadyTrigger");
    
    public void OnJumpStart() => _up = true;
    public void OnJumpEnd() => _up = false;
    
    public void OnHoverStart() => _hover = true;
    public void OnHoverEnd() => _hover = false;
    
    public void OnShootArrowTrigger() => Debug.Log("Arrow Shoot");

    public void OnDownStart() => _down = true;
    public void OnDownEnd() => _down = false;
    
    public void OnLeftStart() => _left = true;
    public void OnLeftEnd() => _left = false;

    


}