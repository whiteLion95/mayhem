using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesStage : MonoBehaviour
{
    public Action<EnemiesStage> OnStageCleared;
    public Action<EnemiesWave> OnWaveToSpawn;

    private List<EnemiesWave> _enemiesWaves;
    private int _curWaveIndex;
    private CoverPlace _coverPlace;

    public CoverPlace CoverPlace => _coverPlace;

    public EnemiesWave CurWave => _enemiesWaves[_curWaveIndex];

    public void Init(CoverPlace coverPlace)
    {
        _enemiesWaves = new List<EnemiesWave>();
        _coverPlace = coverPlace;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EnemiesWave wave))
            {
                _enemiesWaves.Add(wave);
                wave.Init();
                wave.OnWaveCleared += HandleWaveCleared;
            }
        }
    }

    private void HandleWaveCleared()
    {
        if (_curWaveIndex < _enemiesWaves.Count - 1)
        {
            _curWaveIndex++;
            OnWaveToSpawn?.Invoke(_enemiesWaves[_curWaveIndex]);
        }
        else
        {
            OnStageCleared?.Invoke(this);
        }
    }
}
