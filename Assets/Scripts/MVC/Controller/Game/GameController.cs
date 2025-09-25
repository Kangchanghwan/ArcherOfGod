using Model;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private int timer = 60;
    
    
    private void OnEnable()
    {
        EnemyModel.OnEnemyDeath += Win;
        PlayerModel.OnPlayerDeath += Lose;
    }

    private void OnDisable()
    {
        EnemyModel.OnEnemyDeath -= Win;
        PlayerModel.OnPlayerDeath -= Lose;
    }

    private void Lose()
    {
        Debug.Log("Lose");
    }
    private void Win()
    {
        Debug.Log("Win");
    }

}
