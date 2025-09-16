
public class GameManager : Singleton<GameManager>
{
    public ITargetable PlayerOfTarget { get; private set; }
    public ITargetable EnemyOfTarget { get; private set; }

    protected override void Awake()
    {
        PlayerOfTarget = FindAnyObjectByType<Enemy>();
        EnemyOfTarget = FindAnyObjectByType<Player>();
    }
    
    
}
