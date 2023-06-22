using Lean.Pool;
using UnityEngine;

public class PlayerFireController : FireController
{
    [SerializeField]
    private LayerMask _layersToHit;

    private Player _player;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _player = Player.Instance;
    }

    public void Fire(Vector2 screenPos)
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, _layersToHit))
        {
            Projectile newProjectile = _gun.FireTween(hitInfo.point, hitInfo);
            newProjectile.OnTargetPoint -= HandleProjectileOnTarget;
            newProjectile.OnTargetPoint += HandleProjectileOnTarget;
        }

        OnFire?.Invoke();
    }

    private void HandleProjectileOnTarget(Projectile proj, RaycastHit hitInfo)
    {
        proj.OnTargetPoint -= HandleProjectileOnTarget;
        LeanPool.Despawn(proj.gameObject);

        Enemy hitEnemy = hitInfo.collider.GetComponentInParent<Enemy>();

        if (hitEnemy)
        {
            if (hitInfo.collider.Equals(hitEnemy.BodyCollider))
            {
                hitEnemy.TakeDamage(_player.Damage);
            }
            else if (hitInfo.collider.Equals(hitEnemy.HeadCollider))
            {
                Enemy.OnHeadShot?.Invoke(hitEnemy, proj.transform.position);
                hitEnemy.TakeDamage((int)hitEnemy.HealthController.CurHealth);
            }
        }
    }
}
