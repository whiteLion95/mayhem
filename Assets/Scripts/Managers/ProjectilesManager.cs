using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesManager : MonoBehaviour
{
    private List<Projectile> _activeProjectiles = new List<Projectile>();
    private Player _player;

    public Action<Projectile, Collision> OnProjCollision;
    public Action<Projectile, RaycastHit> OnProjOnTarget;

    private void Start()
    {
        _player = Player.Instance;

        _player.OnNewState += HandlePlayerNewState;
    }

    private void OnEnable()
    {
        Gun.OnFire += HandleGunFire;
        Projectile.OnProjDisabled += HandleProjDisabled;
    }

    private void OnDisable()
    {
        Gun.OnFire -= HandleGunFire;
        Projectile.OnProjDisabled -= HandleProjDisabled;
    }

    private void HandleGunFire(Gun gun, Projectile proj)
    {
        _activeProjectiles.Add(proj);
        proj.OnCollision += HandleProjCollision;
        proj.OnTargetPoint += HandleProjOnTarget;
    }

    private void HandleProjDisabled(Projectile proj)
    {
        _activeProjectiles.Remove(proj);
    }

    private void HandlePlayerNewState(Player.State state)
    {
        if (state == Player.State.Running || state == Player.State.Battle)
        {
            for (int i = 0; i < _activeProjectiles.Count; i++)
            {
                Projectile proj = _activeProjectiles[i];
                Destroy(proj.gameObject);
            }
        }
    }

    private void HandleProjCollision(Projectile proj, Collision col)
    {
        proj.OnCollision -= HandleProjCollision;
        OnProjCollision?.Invoke(proj, col);
    }

    private void HandleProjOnTarget(Projectile proj, RaycastHit hitInfo)
    {
        proj.OnTargetPoint -= HandleProjOnTarget;
        OnProjOnTarget?.Invoke(proj, hitInfo);
    }
}
