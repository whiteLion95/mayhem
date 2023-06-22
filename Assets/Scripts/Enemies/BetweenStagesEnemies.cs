using UnityEngine;

public class BetweenStagesEnemies : MonoBehaviour
{
    [SerializeField]
    private EnemiesStage _prevStage;
    [SerializeField]
    private EnemiesStage _nextStage;

    public EnemiesStage PrevStage => _prevStage;
    public EnemiesStage NextStage => _nextStage;

    private EnemiesWave _enemiesWave;

    public EnemiesWave EnemiesWave => _enemiesWave;

    public void Init()
    {
        _enemiesWave = GetComponentInChildren<EnemiesWave>();
        _enemiesWave.Init();
    }
}
