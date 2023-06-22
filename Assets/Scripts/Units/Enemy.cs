using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : Unit
{
    [SerializeField]
    private Collider _bodyCollider;
    [SerializeField]
    private Collider _headCollider;
    [SerializeField]
    private DOTweenAnimation _tweenAnim;
    [SerializeField]
    private float _disappearDelay = 1f;
    [SerializeField]
    private Collider _deathCollider;
    [SerializeField]
    private Vector3 _deathImpactVector;
    [SerializeField]
    private float _deathImpactForce;
    [SerializeField]
    private MultiAimConstraint _handAim;
    [Space(10f)]
    [Header("Enemy behaviour")]
    [SerializeField]
    private bool _lookOutOnSpawn = true;
    [ShowIf("_lookOutOnSpawn")]
    [SerializeField]
    private float _minLookOutDelay = 1f;
    [ShowIf("_lookOutOnSpawn")]
    [SerializeField]
    private float _maxLookOutDelay = 3f;
    [ShowIf("_lookOutOnSpawn")]
    [SerializeField]
    private Vector3 _lookOutOffset;
    [ShowIf("_lookOutOnSpawn")]
    [SerializeField]
    private float _lookOutDuration = 1f;
    [Space(5f)]
    [SerializeField]
    private bool _isHiding;
    [ShowIf("_isHiding")]
    [SerializeField]
    private float _hideDelay = 1f;
    [Space(5f)]
    [SerializeField]
    private bool _isJumping;
    [ShowIf("_isJumping")]
    [SerializeField]
    private float _jumpPower;
    [ShowIf("_isJumping")]
    [SerializeField]
    private float _jumpDuration;
    [ShowIf("_isJumping")]
    [SerializeField]
    private Ease _jumpEase = Ease.Linear;
    [ShowIf("_isJumping")]
    [SerializeField]
    private bool _jumpOnSpawn;
    [ShowIf("_jumpOnSpawn")]
    [SerializeField]
    private Transform _spawnJumpTarget;
    [ShowIf("_jumpOnSpawn")]
    [SerializeField]
    private bool _repeatedJumps;
    [ShowIf("_repeatedJumps")]
    [SerializeField]
    private Transform _repeatedJumpTarget;
    [ShowIf("_repeatedJumps")]
    [SerializeField]
    private float _nextRepeatedJumpDelay = 2f;

    public static Action<Enemy, Vector3> OnHeadShot;

    private Vector3 _originPos;
    private EnemyFireController _fireController;
    private Player _player;
    private GameManager _gameManager;
    private Transform _curJumpTarget;
    private Animator _anim;
    private Rigidbody _rB;

    public Collider BodyCollider => _bodyCollider;
    public Collider HeadCollider => _headCollider;

    protected override void Awake()
    {
        base.Awake();

        _fireController = GetComponent<EnemyFireController>();
        _anim = GetComponentInChildren<Animator>();
        _rB = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();

        Init();
    }

    private void Init()
    {
        _player = Player.Instance;
        _gameManager = GameManager.Instance;

        gameObject.SetActive(false);
        _originPos = transform.position;
        InitHandAim();

        _player.OnDead += HandlePlayerDead;
    }

    private void OnDisable()
    {
        if (_player)
            _player.OnDead -= HandlePlayerDead;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        ActionsOnSpawn();
    }

    private void ActionsOnSpawn()
    {
        if (_lookOutOnSpawn)
        {
            float lookOutDelay = UnityEngine.Random.Range(_minLookOutDelay, _maxLookOutDelay);

            transform.DOMove(_lookOutOffset, _lookOutDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(lookOutDelay)
                .OnComplete(() =>
                {
                    if (_isHiding)
                        StartCoroutine(HidingRoutine());
                    else
                        StartCoroutine(_fireController.RepeatedFire(_player.HitPlace.transform));

                    _tweenAnim.DORestartById("idle");
                });
        }

        if (_isJumping)
        {
            if (_jumpOnSpawn)
            {
                _spawnJumpTarget.SetParent(_gameManager.EnemiesManager.JumpTargetsParent);

                Action onJumpComplete = default;

                if (!_repeatedJumps)
                    onJumpComplete = () =>
                    {
                        _tweenAnim.DORestartById("idle");
                        StartCoroutine(_fireController.RepeatedFire(_player.HitPlace.transform));
                    };
                else
                {
                    _repeatedJumpTarget.SetParent(_gameManager.EnemiesManager.JumpTargetsParent);

                    onJumpComplete = () =>
                    {
                        StartCoroutine(RepeatedJumpsRoutine());
                    };
                }

                Jump(_spawnJumpTarget, onJumpComplete);
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();

        StopAllCoroutines();
        _tweenAnim.DOKill();

        _anim.SetTrigger("die");

        _bodyCollider.enabled = false;
        _headCollider.enabled = false;
        _deathCollider.enabled = true;

        _rB.isKinematic = false;
        _rB.AddForce(_deathImpactVector * _deathImpactForce, ForceMode.Impulse);

        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator HidingRoutine()
    {
        while (!IsDead)
        {
            if (!_player.IsDead)
                StartCoroutine(_fireController.FireRoutine(_player.HitPlace.transform));

            yield return new WaitForSeconds(_hideDelay);
            transform.DOMove(_originPos, _lookOutDuration).SetEase(Ease.Linear);

            yield return new WaitForSeconds(_hideDelay + _lookOutDuration);
            transform.DOMove(_lookOutOffset, _lookOutDuration).SetRelative(true).SetEase(Ease.Linear);

            yield return new WaitForSeconds(_lookOutDuration);
        }
    }

    private IEnumerator RepeatedJumpsRoutine()
    {
        if (!_player.IsDead)
        {
            StartCoroutine(_fireController.FireRoutine(_player.HitPlace.transform));
            yield return new WaitForSeconds(_nextRepeatedJumpDelay);

            Action onJumpComplete = () => StartCoroutine(RepeatedJumpsRoutine());

            if (_curJumpTarget)
            {
                if (_curJumpTarget.Equals(_spawnJumpTarget))
                    Jump(_repeatedJumpTarget, onJumpComplete);
                else if (_curJumpTarget.Equals(_repeatedJumpTarget))
                    Jump(_spawnJumpTarget, onJumpComplete);
            }
        }
    }

    private void Jump(Transform target, Action OnComplete = null)
    {
        _curJumpTarget = target;
        _tweenAnim.DORewind();

        transform.DOJump(target.position, _jumpPower, 1, _jumpDuration).SetEase(_jumpEase)
            .OnComplete(() => OnComplete?.Invoke());

        Vector3 jumpDir = (target.position - transform.position).normalized;
        SetTweenAnim(jumpDir);
    }

    private void SetTweenAnim(Vector3 jumpDir)
    {
        if (Mathf.Abs(jumpDir.x) > Mathf.Abs(jumpDir.z))
        {
            if (jumpDir.x > 0)
            {
                _tweenAnim.DORestartById("rightFlip");
            }
            else
            {
                _tweenAnim.DORestartById("leftFlip");
            }
        }
        else
        {
            if (jumpDir.z > 0)
            {
                _tweenAnim.DORestartById("backFlip");
            }
            else
            {
                _tweenAnim.DORestartById("forwardFlip");
            }
        }
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(_disappearDelay);
        Destroy(gameObject);
    }

    private void HandlePlayerDead(Unit unit)
    {
        StopAllCoroutines();
        _tweenAnim.DOPlayById("idle");
    }

    private void InitHandAim()
    {
        _handAim.data.sourceObjects = new WeightedTransformArray()
        {
            new WeightedTransform(_player.HitPlace.transform, 1f)
        };
    }
}
