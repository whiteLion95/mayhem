using DG.Tweening;
using System;
using UnityEngine;

public class PlayerInBattleBehaviour : MonoBehaviour
{
    [Header("In cover")]
    [SerializeField]
    private Vector3 _inBattleRotation;
    [SerializeField]
    private float _rotationDuration = 0.15f;
    [SerializeField]
    private Ease _rotationEase = Ease.Linear;

    [Space(5f)]
    [Header("Look out from cover")]
    [SerializeField]
    private Vector3 _lookOutRotation;
    [SerializeField]
    private float _lookOutDuration = 0.1f;
    [SerializeField]
    private float _goBackToCoverDelay = 0.15f;

    public Action OnLookingOutFromCover;
    public Action OnOutFromCover;
    public Action OnGoingBackToCover;
    public Action OnCover;

    private Player _player;
    private PlayerFireController _fireController;
    private GameManager _gameManager;
    private bool _isOutOfCover;
    private bool _isLookingOutOfCover;
    private Collider _playerCollider;

    public bool IsOutOfCover => _isOutOfCover;
    public bool IsLookingOutOfCover => _isLookingOutOfCover;

    public void Init()
    {
        _player = Player.Instance;
        _gameManager = GameManager.Instance;
        _fireController = GetComponent<PlayerFireController>();
        _playerCollider = GetComponentInChildren<Collider>();

        _player.OnNewState += HandlePlayerNewState;
        _fireController.OnFire += HandleFire;
    }

    private void HandlePlayerNewState(Player.State newState)
    {
        if (newState == Player.State.Battle)
        {
            _player.PlayerModel.DORotate(_inBattleRotation, _rotationDuration).SetEase(_rotationEase);
            _playerCollider.enabled = false;
        }

        if (newState == Player.State.Dead)
            Destroy(this);

        if (newState == Player.State.Running)
            _playerCollider.enabled = true;
    }

    public void LookOutFromCover(Action onOutOfCover = null)
    {
        if (!_isLookingOutOfCover)
        {
            _isLookingOutOfCover = true;
            OnLookingOutFromCover?.Invoke();

            CoverPlace curCovPlace = _gameManager.CoversManager.CurCoverPlace;
            Vector3 curCoverPos = curCovPlace.transform.position;
            float xOffset = curCovPlace.LookOutXOffset;
            Vector3 lookOutPos = new Vector3(curCoverPos.x + xOffset, curCoverPos.y, curCoverPos.z);

            _player.PlayerModel.DOKill();
            _player.PlayerModel.DOMove(lookOutPos, _lookOutDuration).SetEase(Ease.Linear);
            _player.PlayerModel.DORotate(_lookOutRotation, _lookOutDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    OnOutFromCover?.Invoke();
                    onOutOfCover?.Invoke();
                    _isOutOfCover = true;
                });

            _playerCollider.enabled = true;
        }
    }

    public void GoBackToCover()
    {
        CoverPlace curCovPlace = _gameManager.CoversManager.CurCoverPlace;

        _player.PlayerModel.DOMove(curCovPlace.transform.position, _lookOutDuration).SetDelay(_goBackToCoverDelay).SetEase(Ease.Linear);
        _player.PlayerModel.DORotate(_inBattleRotation, _lookOutDuration).SetDelay(_goBackToCoverDelay).SetEase(Ease.Linear)
            .OnPlay(() =>
            {
                _isOutOfCover = false;
                OnGoingBackToCover?.Invoke();
            }).OnComplete(() =>
            {
                _isLookingOutOfCover = false;
                OnCover?.Invoke();

                if (_player.CurState == Player.State.Battle)
                    _playerCollider.enabled = false;
            });
    }

    private void HandleFire()
    {
        if (_isOutOfCover && _player.CurState == Player.State.Battle)
        {
            _player.PlayerModel.DOKill();
        }
    }
}
