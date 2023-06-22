using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    public Action<Projectile, Collision> OnCollision;
    public Action<Projectile, Collider> OnTrigger;
    public Action<Projectile, RaycastHit> OnTargetPoint;
    public static Action<Projectile> OnProjDisabled;

    public Gun MyGun { get; private set; }
    public Rigidbody RB { get; private set; }
    public int Damage => _damage;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        OnProjDisabled?.Invoke(this);
        ResetRigidbody();
    }

    public void Initialize(Gun gun)
    {
        MyGun = gun;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(this, other);
    }

    private void ResetRigidbody()
    {
        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
    }
}