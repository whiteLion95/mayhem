using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    protected int _damage = 1;

    public Action<Unit> OnDead;

    protected HealthController _healthController;

    public bool IsDead { get; protected set; }
    public int Damage => _damage;
    public HealthController HealthController => _healthController;

    protected virtual void Awake()
    {
        _healthController = GetComponent<HealthController>();
    }

    protected virtual void Start()
    {
        _healthController.OnZeroHealth += HandleZeroHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        _healthController.TakeDamage(damage);
    }

    protected virtual void Die()
    {
        IsDead = true;
        OnDead?.Invoke(this);
    }

    protected virtual void HandleZeroHealth()
    {
        Die();
    }
}
