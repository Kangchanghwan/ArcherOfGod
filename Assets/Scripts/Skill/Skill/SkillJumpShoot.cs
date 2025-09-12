using UnityEngine;
using System.Collections;

public class SkillJumpShoot : SkillBase
{
    [Header("Skill Settings")] 
    [SerializeField] private Arrow arrow;
    
    [SerializeField] private float jumpHeight;
    [SerializeField] private int damage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Vector2 fireOffset;

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
        _originalPosition = Rb.transform.position;

        CurrentSkillCoroutine = StartCoroutine(ExecuteJump());
        yield return CurrentSkillCoroutine;
    }

    public override string GetAnimationName() => "JumpShoot";

    private IEnumerator ExecuteJump()
    {
        // 상승 전 대기 단계
        yield return new WaitUntil(() => _up); // up이 참이 될때까지 기다림
        // 상승 단계
        yield return StartCoroutine(RisePhase());
        // 공중에서 정지 단계
        yield return new WaitUntil(() => _down);
        // 하강 단계
        yield return StartCoroutine(FallPhase());
    }


    private IEnumerator RisePhase()
    {
        Rb.bodyType = RigidbodyType2D.Kinematic;

        float peak = _originalPosition.y + jumpHeight;
        Rb.transform.position = new Vector2(_originalPosition.x, peak);
        yield return new WaitUntil(() => _hover);
    }


    private IEnumerator FallPhase()
    {
        Rb.transform.position = new Vector2(_originalPosition.x, _originalPosition.y);

        Rb.bodyType = RigidbodyType2D.Dynamic;
        Rb.linearVelocity = Vector2.zero;
        yield return new WaitUntil(() => _left);
    }

    public void OnJumpReadyTrigger() => Debug.Log("OnJumpReadyTrigger");

    public void OnJumpStart() => _up = true;
    public void OnJumpEnd() => _up = false;

    public void OnHoverStart() => _hover = true;
    public void OnHoverEnd() => _hover = false;

    public void OnShootArrowTrigger() =>
        Arrow(
            PoolObject(arrow.gameObject)
                .GetComponent<Arrow>()
            );

    public void OnDownStart() => _down = true;
    public void OnDownEnd() => _down = false;

    public void OnLeftStart() => _left = true;
    public void OnLeftEnd() => _left = false;

    private void Arrow(Arrow arrow)
    {
        arrow.gameObject.SetActive(true);
        Vector2 p0 = (Vector2)transform.position + fireOffset;
        Vector2 p1 = p0;
        Vector2 p2 = Target.position;
        arrow.Duration = arrowSpeed;
        arrow.ShotArrow(p0, p1, p2);
    }
}