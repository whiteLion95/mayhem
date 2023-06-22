using DG.Tweening;
using Lean.Pool;
using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunDataSO _data;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private bool _limitedBullets;
    [SerializeField] private bool _reloadOnEmpty;

    public static Action<Gun, Projectile> OnFire;
    public Action OnReloaded;
    public Action OnNotEnoughBullets;
    public Action OnEmpty;

    private int _curBulCount;

    public int CurBulCount { get => _curBulCount; }
    public Transform FirePoint => _firePoint;
    public GunDataSO Data => _data;

    private void Start()
    {
        _curBulCount = _data.BulletsCount;
    }

    public Projectile FireRB(Vector3 direction, Projectile projPrefab = null, float customForce = 0f)
    {
        if (IsEnoughBullets())
        {
            Projectile newProjectile;

            if (projPrefab == null)
                newProjectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.LookRotation(direction));
            else
                newProjectile = Instantiate(projPrefab, _firePoint.position, Quaternion.LookRotation(direction));

            newProjectile.Initialize(this);

            if (customForce == 0f)
                newProjectile.RB.AddForce(direction * _data.FirePower, ForceMode.Impulse);
            else
                newProjectile.RB.AddForce(direction * customForce, ForceMode.Impulse);

            OnFire?.Invoke(this, newProjectile);
            return newProjectile;
        }

        return null;
    }

    public Projectile FireTween(Vector3 targetPoint, RaycastHit hitInfo)
    {
        if (IsEnoughBullets())
        {
            Projectile newProjectile = LeanPool.Spawn(_projectilePrefab, _firePoint.position, Quaternion.LookRotation((targetPoint - _firePoint.position).normalized));
            newProjectile.Initialize(this);

            newProjectile.transform.DOMove(targetPoint, _data.BulletTweenSpeed).SetSpeedBased(true).SetEase(Ease.Linear)
                .OnComplete(() => newProjectile.OnTargetPoint?.Invoke(newProjectile, hitInfo));

            OnFire?.Invoke(this, newProjectile);
            return newProjectile;
        }

        return null;
    }

    private bool IsEnoughBullets()
    {
        if (_limitedBullets)
        {
            if (_curBulCount > 0)
            {
                _curBulCount--;

                if (_curBulCount == 0)
                {
                    OnEmpty?.Invoke();
                    StartCoroutine(ReloadRoutine());
                }

                if (!_reloadOnEmpty)
                    StartCoroutine(nameof(ReloadRoutine));

                return true;
            }
            else
            {
                OnNotEnoughBullets?.Invoke();
                return false;
            }
        }

        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        while (_curBulCount < _data.BulletsCount)
        {
            yield return new WaitForSeconds(_data.ReloadTime);

            _curBulCount += _data.BulletsPerReload;

            if (_curBulCount > _data.BulletsCount)
                _curBulCount = _data.BulletsCount;

            OnReloaded?.Invoke();
        }
    }
}