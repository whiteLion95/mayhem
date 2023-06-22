using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesWave : MonoBehaviour
{
    public Action OnWaveCleared;

    private List<Enemy> _enemies;

    public void Init()
    {
        _enemies = new List<Enemy>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Enemy enemy))
            {
                _enemies.Add(enemy);
            }
        }

        foreach (var enemy in _enemies)
        {
            enemy.OnDead += HandleEnemyDead;
        }
    }

    public void SpawnEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Spawn();
        }
    }

    public void DespawnEnemies()
    {
        foreach (var enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private void HandleEnemyDead(Unit unit)
    {
        _enemies.Remove(unit as Enemy);

        if (_enemies.Count == 0)
            OnWaveCleared?.Invoke();
    }
}
