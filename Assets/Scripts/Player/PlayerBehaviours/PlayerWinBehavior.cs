using DG.Tweening;
using UnityEngine;

public class PlayerWinBehavior : MonoBehaviour
{
    [SerializeField]
    private Vector3 _winRotation;
    [SerializeField]
    private float _rotationDuration;

    private GameManager _gameManager;
    private Player _player;

    public void Init()
    {
        _gameManager = GameManager.Instance;
        _player = Player.Instance;

        _gameManager.OnWin += HandleWin;
    }

    private void HandleWin()
    {
        _player.PlayerModel.DORotate(_winRotation, _rotationDuration).SetEase(Ease.Linear);
    }
}
