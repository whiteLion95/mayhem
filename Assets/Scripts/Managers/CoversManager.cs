using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoversManager : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCoversContainer;

    public Action<CoverPlace> OnNextCoverPlaceSet;

    private List<CoverPlace> _coverPlaces;
    private int _curCovPlaceIndex = -1;
    private Player _player;
    private GameManager _gameManager;

    public List<CoverPlace> CoverPlaces => _coverPlaces;
    public CoverPlace CurCoverPlace => _coverPlaces[_curCovPlaceIndex];
    public int CurCoverPlaceIndex => _curCovPlaceIndex;

    public void Init()
    {
        _coverPlaces = new List<CoverPlace>();

        foreach (Transform child in _playerCoversContainer)
        {
            CoverPlace coverPlace = child.GetComponentInChildren<CoverPlace>();

            if (coverPlace)
                _coverPlaces.Add(coverPlace);
        }

        if (_coverPlaces.Count == 0)
            Debug.LogWarning("Cover places of CoversManager are empty!");
        else
            SetNextCoverPlace();

        _player = Player.Instance;
        _gameManager = GameManager.Instance;

        _gameManager.EnemiesManager.OnStageClearedGlobal += HandleStageCleared;
    }

    public void SetNextCoverPlace()
    {
        if (_curCovPlaceIndex < _coverPlaces.Count - 1)
        {
            _curCovPlaceIndex++;
            OnNextCoverPlaceSet?.Invoke(CurCoverPlace);
        }
        else
        {
            _gameManager.OnAllStagesComplete?.Invoke();
        }
    }

    private void HandleStageCleared(EnemiesStage stage)
    {
        Invoke(nameof(SetNextCoverPlace), 0.2f);
    }
}