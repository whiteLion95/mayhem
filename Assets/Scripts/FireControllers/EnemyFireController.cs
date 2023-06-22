using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyFireController : FireController
{
    [SerializeField]
    private float _minRepeatedFireDelay = 0.5f;
    [SerializeField]
    private float _maxRepeatedFireDelay = 1.5f;
    [SerializeField]
    private float _fireOffsetRange = 1f;
    [SerializeField]
    private float _redProjChance = 0.3f;
    [SerializeField]
    private Projectile _redProjPrefab;
    [SerializeField]
    private float _redProjForce = 15f;
    [SerializeField]
    private DOTweenAnimation _fireTween;

    private Enemy _enemy;
    private bool _redProjectileFired;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();

        _enemy = GetComponent<Enemy>();
    }

    protected override void Start()
    {
        base.Start();

        _player = Player.Instance;

        _player.OnDead += HandlePlayerDead;
    }

    private void OnDisable()
    {
        if (_player)
            _player.OnDead -= HandlePlayerDead;
    }

    public IEnumerator RepeatedFire(Transform targetPoint)
    {
        while (!_player.IsDead && !_enemy.IsDead)
        {
            _redProjectileFired = false;
            float fireDelay = UnityEngine.Random.Range(_minRepeatedFireDelay, _maxRepeatedFireDelay);
            yield return new WaitForSeconds(fireDelay);
            _fireTween.DORestartById("fire");

            for (int i = 0; i < _gun.Data.BulletsCount; i++)
            {
                Fire(targetPoint.position);
                yield return new WaitForSeconds(_gun.Data.RapidFireDelay);
            }

            _fireTween.DORestartById("fireRewind");
        }
    }

    public IEnumerator FireRoutine(Transform targetPoint)
    {
        _redProjectileFired = false;
        _fireTween.DORestartById("fire");

        for (int i = 0; i < _gun.Data.BulletsCount; i++)
        {
            Fire(targetPoint.position);
            yield return new WaitForSeconds(_gun.Data.RapidFireDelay);
        }

        _fireTween.DORestartById("fireRewind");
    }

    private void Fire(Vector3 targetPoint)
    {
        float xRandOffset = UnityEngine.Random.Range(-_fireOffsetRange, _fireOffsetRange);
        float yRandOffset = UnityEngine.Random.Range(-_fireOffsetRange, _fireOffsetRange);

        Vector3 randPoint = new Vector3(targetPoint.x + xRandOffset, targetPoint.y + yRandOffset, targetPoint.z);
        Vector3 fireDir = (randPoint - _gun.FirePoint.position).normalized;

        Projectile newProjectile = null;

        if (!_redProjectileFired && RedProjectileCheck())
        {
            _redProjectileFired = true;
            newProjectile = _gun.FireRB((targetPoint - _gun.FirePoint.position).normalized, _redProjPrefab, _redProjForce);
        }
        else
            newProjectile = _gun.FireRB(fireDir);

        if (newProjectile != null)
        {
            newProjectile.OnCollision -= HandleProjectileCollision;
            newProjectile.OnCollision += HandleProjectileCollision;
        }
    }

    private bool RedProjectileCheck()
    {
        float result = UnityEngine.Random.Range(0f, 1f);

        if (result <= _redProjChance)
            return true;

        return false;
    }

    private void HandleProjectileCollision(Projectile proj, Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _player.TakeDamage(proj.Damage);
        }

        Destroy(proj.gameObject);
    }

    private void HandlePlayerDead(Unit unit)
    {
        StopAllCoroutines();
    }
}
