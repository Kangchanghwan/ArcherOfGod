using UnityEngine;

public class LinearArrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float speed = 20f;            // 화살 속도
    [SerializeField] private float lifeTime = 5f;
    
    private Vector2 direction;
    private float currentSpeed;
    private bool hasHit = false;
    private bool isFlying = false;
    private GameObject owner;
    
    public void Launch(Vector2 launchDirection, GameObject owner, float launchSpeed = -1f)
    {
        direction = launchDirection.normalized;
        currentSpeed = launchSpeed > 0 ? launchSpeed : speed;
        this.owner = owner;
        
        isFlying = true;
        
        // 화살이 발사 방향을 바라보도록 회전
        SetRotationFromDirection(direction);
        
        // 자동 삭제
        Destroy(gameObject, lifeTime);
        
        Debug.Log($"Linear Arrow launched: direction={direction}, speed={currentSpeed}");
    }
    
    private void Update()
    {
        if (!isFlying || hasHit) return;
        
        // Transform 기반 이동
        float moveDistance = currentSpeed * Time.deltaTime;
        Vector3 movement = direction * moveDistance;
        transform.position += movement;
        
        // 이동 방향으로 회전 (필요시)
        SetRotationFromDirection(direction);
    }
    
    private void SetRotationFromDirection(Vector2 dir)
    {
        if (dir.magnitude > 0.01f)
        {
            // 화살이 위쪽을 기본으로 바라보는 경우
            transform.up = dir.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHit) return;

        var entity = other.collider.GetComponent<Entity>();

        if (entity != null && owner != null && entity.gameObject.name != owner.name)
        {
            HitTarget(entity);
        }

        var ground = other.collider.GetComponent<Ground>();

        if (ground != null)
        {
            HitGround();
        }
    }

    private void HitTarget(Entity entity)
    {
        hasHit = true;
        isFlying = false;
        
        // 충돌 효과
        GetComponent<Collider2D>().enabled = false;
        entity.health.TakeDamage(damage);

        // 잠깐 후 삭제
        ObjectPool.instance.ReturnObject(gameObject, 1f);
    }

    private void HitGround()
    {
        hasHit = true;
        isFlying = false;
        
        GetComponent<Collider2D>().enabled = false;
        
        Debug.Log("Linear Arrow hit ground!");
        
        // 땅에 박힌 후 삭제
        ObjectPool.instance.ReturnObject(gameObject, 1f);
    }
    

}
