using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private Transform _enemiesStagesContainer;
    [SerializeField]
    private Transform _betweenStagesEnemiesContainer;
    [SerializeField]
    private float _waveSpawnDelay;
    [SerializeField]
    private Transform _jumpTargetsParent;

    public Action<EnemiesStage> OnStageClearedGlobal;

    private List<EnemiesStage> _enemiesStages;
    private List<BetweenStagesEnemies> _betweenStagesEnemies;
    private GameManager _gameManager;
    private Player _player;
    private EnemiesStage _curStage;

    public Transform JumpTargetsParent => _jumpTargetsParent;

    public void Init()
    {
        _gameManager = GameManager.Instance;
        _player = Player.Instance;

        _enemiesStages = new List<EnemiesStage>(_enemiesStagesContainer.GetComponentsInChildren<EnemiesStage>());
        _betweenStagesEnemies = new List<BetweenStagesEnemies>(_betweenStagesEnemiesContainer.GetComponentsInChildren<BetweenStagesEnemies>());

        for (int i = 0; i < _enemiesStages.Count; i++)
        {
            EnemiesStage stage = _enemiesStages[i];

            if (_gameManager.CoversManager.CoverPlaces.Count > i)
                stage.Init(_gameManager.CoversManager.CoverPlaces[i]);

            stage.OnStageCleared += HandleStageCleared;
        }

        foreach (var waveBetweenStages in _betweenStagesEnemies)
        {
            waveBetweenStages.Init();
        }

        _player.OnNewState += HandlePlayerState;
    }

    private IEnumerator SpawnEnemyWave(EnemiesWave wave)
    {
        yield return new WaitForSeconds(_waveSpawnDelay);
        wave.SpawnEnemies();
    }

    private void SpawnBetweenStagesWave(EnemiesStage stage)
    {
        if (_betweenStagesEnemies.Count > 0)
        {
            BetweenStagesEnemies bse = _betweenStagesEnemies
            .Find((bse) => bse.PrevStage.Equals(stage));

            if (bse != null)
            {
                StartCoroutine(SpawnEnemyWave(bse.EnemiesWave));
            }
        }
    }

    private void DestroyBetweenStagesWave(EnemiesStage stage)
    {
        if (_betweenStagesEnemies.Count > 0)
        {
            BetweenStagesEnemies bse = _betweenStagesEnemies
            .Find((bse) => bse.NextStage.Equals(stage));

            if (bse != null)
            {
                bse.EnemiesWave.DespawnEnemies();
                _betweenStagesEnemies.Remove(bse);
            }
        }
    }

    private void HandlePlayerState(Player.State state)
    {
        if (state == Player.State.Battle)
        {
            _curStage = _enemiesStages[_gameManager.CoversManager.CurCoverPlaceIndex];
            _curStage.OnWaveToSpawn += HandleWaveToSpawn;
            DestroyBetweenStagesWave(_curStage);
            StartCoroutine(SpawnEnemyWave(_curStage.CurWave));
        }
    }

    private void HandleStageCleared(EnemiesStage stage)
    {
        OnStageClearedGlobal?.Invoke(stage);
        SpawnBetweenStagesWave(stage);
    }

    private void HandleWaveToSpawn(EnemiesWave wave)
    {
        StartCoroutine(SpawnEnemyWave(wave));
    }
}
