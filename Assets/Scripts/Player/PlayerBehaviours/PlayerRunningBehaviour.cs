using DG.Tweening;
using System;
using UnityEngine;

public class PlayerRunningBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 4f;
    [SerializeField]
    private Vector3 _runRotation;
    [SerializeField]
    private float _rotationDuration = 0.15f;
    [SerializeField]
    private Ease _rotationEase = Ease.Linear;

    private Player _player;
    private GameManager _gameManager;

    public float MoveSpeed => _moveSpeed;

    public void Init()
    {
        _player = Player.Instance;
        _gameManager = GameManager.Instance;

        _player.OnNewState += HandlePlayerNewState;
    }

    private void RunTo(Transform target, Action onComplete)
    {
        _player.PlayerModel.DORotate(_runRotation, _rotationDuration).SetEase(_rotationEase);

        _player.transform.DOMove(target.position, _moveSpeed).SetSpeedBased(true).SetEase(Ease.Linear)
            .OnComplete(() => onComplete?.Invoke());
    }

    private void HandlePlayerNewState(Player.State newState)
    {
        if (newState == Player.State.Running)
        {
            RunTo(_gameManager.CoversManager.CurCoverPlace.transform, () => _player.SetState(Player.State.Battle));
        }

        if (newState == Player.State.RunningToFinish)
        {
            RunTo(_gameManager.FinishPlace, () => _gameManager.OnWin?.Invoke());
        }
    }

    public void Stop()
    {
        _player.transform.DOKill();
    }
}
