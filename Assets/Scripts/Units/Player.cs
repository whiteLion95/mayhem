using Lean.Touch;
using System;
using UnityEngine;

public class Player : Unit
{
    public enum State
    {
        Default,
        Running,
        Battle,
        Dead,
        RunningToFinish
    }

    [SerializeField]
    private Transform _playerModel;


    public Action<CoverPlace> OnCoverPlace;
    public Action<State> OnNewState;

    private GameManager _gameManager;
    private PlayerAimingBehaviour _aiming;
    private PlayerInBattleBehaviour _battling;
    private PlayerFireController _fireController;
    private PlayerAnimationsController _animController;
    private PlayerHitPlace _hitPlace;
    private Gun _gun;
    private CamerasManager _camManager;
    private PlayerRunningBehaviour _running;
    private PlayerWinBehavior _winBehavior;

    public State CurState { get; private set; }
    public Transform PlayerModel => _playerModel;
    public static Player Instance { get; private set; }
    public PlayerInBattleBehaviour Battling => _battling;
    public PlayerHitPlace HitPlace => _hitPlace;
    public PlayerFireController FireController => _fireController;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        _aiming = GetComponent<PlayerAimingBehaviour>();
        _battling = GetComponent<PlayerInBattleBehaviour>();
        _fireController = GetComponent<PlayerFireController>();
        _hitPlace = GetComponentInChildren<PlayerHitPlace>();
        _animController = GetComponentInChildren<PlayerAnimationsController>();
        _gun = GetComponent<Gun>();
        _running = GetComponent<PlayerRunningBehaviour>();
        _winBehavior = GetComponent<PlayerWinBehavior>();
    }

    protected override void Start()
    {
        base.Start();

        _camManager = CamerasManager.Instance;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }

    public void Init()
    {
        _gameManager = GameManager.Instance;

        _running.Init();
        _battling.Init();
        _animController.Init();
        _winBehavior.Init();

        _gameManager.CoversManager.OnNextCoverPlaceSet += HandleNextCoverPlaceSet;
        _gameManager.OnAllStagesComplete += HandleAllStagesComplete;
    }

    public void SetState(State state)
    {
        CurState = state;
        OnNewState?.Invoke(state);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();

        SetState(State.Dead);
        _running.Stop();
        _gameManager.OnLose?.Invoke();
    }

    #region event handlers
    private void HandleNextCoverPlaceSet(CoverPlace coverPlace)
    {
        SetState(State.Running);
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (!IsDead)
        {
            if (CurState == State.Running)
                _aiming.Aim(finger);

            if (CurState == State.Battle)
            {
                if (_battling.IsOutOfCover)
                {
                    _aiming.Aim(finger);
                }
            }
        }
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (!IsDead)
        {
            if (CurState == State.Battle || CurState == State.Running)
            {
                if (CurState == State.Battle)
                {
                    if (!_battling.IsLookingOutOfCover)
                        _battling.LookOutFromCover(() => _fireController.Fire(finger.ScreenPosition));
                    else if (_battling.IsOutOfCover)
                        _fireController.Fire(finger.ScreenPosition);
                }

                if (CurState == State.Running)
                {
                    _fireController.Fire(finger.ScreenPosition);
                    _animController.DoRunAimAnimation(true);
                }
            }
        }
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        if (!IsDead)
        {
            if (CurState == State.Battle || CurState == State.Running)
            {
                if (CurState == State.Battle)
                    _battling.GoBackToCover();

                if (CurState == State.Running)
                    _animController.DoRunAimAnimation(false);

                _aiming.UnAim();
            }
        }
    }

    private void HandleAllStagesComplete()
    {
        SetState(State.RunningToFinish);
        _aiming.UnAim();

        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
    #endregion
}
