
using Util;

public class GameManager : Singleton<GameManager>
{
    public ITargetable PlayerOfTarget { get; private set; }
    public ITargetable EnemyOfTarget { get; private set; }

    private void Start()
    {
        PlayerOfTarget = FindAnyObjectByType<Enemy>();
        EnemyOfTarget = FindAnyObjectByType<Player>();
    }
    
}
